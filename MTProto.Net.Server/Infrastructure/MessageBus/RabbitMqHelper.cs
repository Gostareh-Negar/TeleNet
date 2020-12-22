using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Infrastructure.Configurations;
using MTProto.NET.Server.Infrastructure.Implementations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	class WaitModel
	{
		private TaskCompletionSource<object> tasksource;
		public Guid Id;
		public Task<object> Task => this.tasksource.Task;
		public void SetObject(object obj)
		{
			this.tasksource.SetResult(obj);
		}
		public void SetException(Exception exp)
		{
			this.tasksource.SetException(exp);
		}
		public WaitModel()
		{
			this.Id = new Guid();
			tasksource = new TaskCompletionSource<object>();
		}
	}
	class RabbitMqHelper
	{

		private BusOptions options;
		private Bus bus;
		private IConnection rabbitMqConnection;
		private IModel requestChannel;
		private IModel replyChaannel;
		private IBasicProperties replyChannelProps;
		private string uniqueId;
		private ConcurrentDictionary<Guid, WaitModel> waitingList = new ConcurrentDictionary<Guid, WaitModel>();
		private readonly ILogger logger;
		public RabbitMqHelper(ILogger<RabbitMqHelper> logger)
		{

		}
		
		public bool IsActive { get; private set; }


		public string GetUniqueId()
		{
			if (string.IsNullOrEmpty(uniqueId))
				uniqueId = Guid.NewGuid().ToString();
			return uniqueId;
		}

		public Task<RabbitMqHelper> Initialize(Bus bus, BusOptions options)
		{
			this.bus = bus;
			this.options = options;
			//this.waitingList = waitingList;
			try
			{
				this.IsActive = this.GetConnection(true) != null;
				this.IsActive = this.IsActive && this.GetRequestChannel() != null;
				this.GetReplyChannel();
			}
			catch (Exception err)
			{
				this.IsActive = false;
			}

			return Task.FromResult(this);
		}
		public async Task FireOnReply(IModel channel, object model, BasicDeliverEventArgs ea)
		{
			await Task.CompletedTask;
			var body = ea.Body.ToArray();
			var response = Encoding.UTF8.GetString(body);

			if (ea.BasicProperties != null && Guid.TryParse(ea.BasicProperties.CorrelationId, out var tmp) && this.waitingList.TryRemove(tmp, out var wait))
			{
				try
				{
					var message = MessageContext.Create(Encoding.UTF8.GetString(ea.Body.ToArray()));
					wait.SetObject(message);
				}
				catch (Exception err)
				{
					wait.SetException(err);
				}
			}
		}
		public async Task FireOnReuest(IModel channel, object model, BasicDeliverEventArgs ea)
		{
			var props = ea.BasicProperties;
			var replyProps = channel.CreateBasicProperties();
			replyProps.CorrelationId = props.CorrelationId;
			try
			{
				/// We have recived a message.
				/// 
				var should_ignore = ea.BasicProperties != null && ea.BasicProperties.AppId == this.GetUniqueId();
				if (!should_ignore)
				{
					var text = Encoding.UTF8.GetString(ea.Body.ToArray());
					var message = MTServer.Services.DeserializeMessage(text);
					var _response = await this.bus.Send(message, mode: SendModes.InternalOnly);
					if (_response != null)
					{
						var responseBytes = Encoding.UTF8.GetBytes(message.Serialize());// container.Body.ToArray();
						channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
						  basicProperties: replyProps, body: responseBytes);
					}
				}
				channel.BasicAck(deliveryTag: ea.DeliveryTag,
				  multiple: false);
			}
			catch (Exception e)
			{
			}
			finally
			{
			}

		}
		public IModel GetRequestChannel(bool refersh = false)
		{
			var que_name = this.options.RabbitMq.RequestQueName;
			//this.respQueue.Take()
			if (this.requestChannel == null || refersh)
			{
				var connection = this.GetConnection(refersh);
				this.requestChannel = connection.CreateModel();
				var channel = this.requestChannel;
				channel.QueueDeclare(queue: que_name,
									 durable: true, exclusive: false, autoDelete: false);

				channel.BasicQos(0, 1, false);

				channel.ExchangeDeclare(exchange: que_name, type: "direct", durable: true, autoDelete: false);
				channel.QueueBind(queue: que_name, exchange: que_name, "rpc");
				var consumer = new EventingBasicConsumer(this.requestChannel);
				channel.BasicConsume(queue: que_name,
					autoAck: false, consumer: consumer);
				consumer.Received += async (model, ea) =>
				{
					await FireOnReuest(channel, model, ea);
				};
			}
			return this.requestChannel;
		}

		public IModel GetReplyChannel(bool refersh = false)
		{
			if (this.replyChaannel == null || refersh)
			{
				var channel = this.GetConnection().CreateModel();
				//channel.QueueDeclare();
				var que_name = "rpc_reply" + Guid.NewGuid().ToString();
				try
				{
					channel.QueueDeclare(queue: que_name);
				}
				catch
				{

				}
				var consumer = new EventingBasicConsumer(channel);

				var props = channel.CreateBasicProperties();
				var correlationId = Guid.NewGuid().ToString();
				props.CorrelationId = correlationId;
				props.ReplyTo = que_name;
				consumer.Received += async (model, ea) =>
				{
					await this.FireOnReply(channel, model, ea);
				};
				channel.BasicConsume(consumer: consumer, queue: que_name, autoAck: true);
				this.replyChannelProps = props;
				this.replyChaannel = channel;
			}
			return this.replyChaannel;
		}
		public IConnection GetConnection(bool refersh = false)
		{
			if (this.rabbitMqConnection == null || refersh)
			{
				var factory = new ConnectionFactory
				{
					HostName = this.options.RabbitMq.Server,
					VirtualHost = this.options.RabbitMq.VirtualHost,
					UserName = this.options.RabbitMq.UserName,
					Password = this.options.RabbitMq.Password
				};
				this.rabbitMqConnection = factory.CreateConnection();
				this.replyChaannel?.Dispose();
				this.replyChaannel = null;
				this.requestChannel?.Dispose();
				this.requestChannel = null;

			}
			return this.rabbitMqConnection;
		}

		public async Task<object> Send(ReadOnlyMemory<byte> body)
		{
			if (!this.IsActive)
			{
				throw new Exception("RabbitMQ is not active");
			}
			
			var waitModel = new WaitModel();
			this.waitingList.TryAdd(waitModel.Id, waitModel);
			var channel = this.GetRequestChannel();
			var props = channel.CreateBasicProperties();
			props.CorrelationId = waitModel.Id.ToString();
			props.ReplyTo = this.replyChannelProps.ReplyTo;
			props.AppId = this.GetUniqueId();

			channel.BasicPublish(
				exchange: this.options.RabbitMq.RequestQueName,
				routingKey: "rpc",
				basicProperties: props,// this.replyChannelProps,
				body: body);
			var result = await waitModel.Task;
			return result;
		}
	}
}
