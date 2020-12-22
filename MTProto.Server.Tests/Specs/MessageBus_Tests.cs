using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Schema.TL.Stats;
using Microsoft.Extensions.Logging;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MTProto.Server.Tests.Helpers;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Server.Infrastructure.Serialization;

namespace MTProto.Server.Tests
{
	[TestClass]
	public class MessageBus_Tests : TestFixture
	{
		[TestMethod]
		public async Task how_message_bus_works()
		{
			//var bb = (new A<string>()) as A<object> as D;
			



			var host = this.CreateWebHost(b =>
			{
				b.AddBus();
				b.ServiceCollection.AddTransient<IMessageHandler, HH>();
			});

			var bus = host.Services.GetService<IBus>();
			await bus.Subscribe(s =>
			{
				s.IsRequestHandler = true;
				s.Topic = "test_topic";
				s.Handler = x =>
				{
					return s.Handler.Ok();
				};
			});
			//await bus.Subscribe<IMessageContext<string>>(s => {

			//	s.IsRequestHandler = true;
			//	s.Handler = x =>
			//	{
			//		return Task.FromResult<object>("ok");
			//	};

			//});
			await host.StartAsync();

			await bus.Publish("", "test_topic");
			var res = await bus.Send("", "test_topic");
			await bus.Publish("");

			TLBroadcastStats recived = null;
			await bus.Subscribe<MTProto.NET.Schema.TL.Stats.TLBroadcastStats>(cfg =>
			{
				cfg.Handler = x =>
				{
					recived = x.Body;
					return Task.FromResult<object>("");
				};
			});
			var logger = host.Services.GetService<ILogger>();
			await bus.Publish(new TLBroadcastStats());
			await host.StartAsync();

			await bus.ProtoSubscribe<TLBroadcastStats>(s =>
			{
				s.Handler = x =>
				{
					//var t = x.Envelop.ProtoType();
					return Task.FromResult<object>(true);
				};
			});

		}
		[TestMethod]
		public async Task remote_rpc_should_work()
		{
			var host = this.CreateWebHost();
			var client = AppDomainHelper.Create("test1").GetProxy<TestClient>();
			client.Start(2535);
			await host.StartAsync();
			var bus = host.Services.GetService<IBus>();

			//await bus.ProtoRegister<MTReqPq>(true,subs =>
			//{
			//	subs.Handler = x => {

			//		return Task.FromResult<object>(new MTReqDhParams {
			//			Nonce = new Org.BouncyCastle.Math.BigInteger(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, }),
			//			EncryptedData = new byte[] { 1, 2, 3 },
			//			P = new byte[] { 1, 2, 3 },
			//			PublicKeyFingerprint = 0,
			//			Q = new byte[] { 1, 2, 3 },
			//			ServerNonce = new Org.BouncyCastle.Math.BigInteger( new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, })
			//		}) ;
			//	};

			//});

			var req = new MTReqPq()
			{ Nonce = new Org.BouncyCastle.Math.BigInteger("12332432543546546456456546456456456456", 10) };


			var context = MessageContext.Create(req);

			var reply = await bus.SendProto(message: context, mode: SendModes.ExternalOnly) ;
			//var obj = reply.B.GetMessage();
			var f = reply.Body;
			



			await host.StopAsync();
			await Task.Delay(10 * 60 * 1000);



		}
		[TestMethod]
		public async Task rabbit_mq()
		{
			await Task.CompletedTask;
			var factory = new ConnectionFactory { HostName = "172.16.0.13", VirtualHost = "test", UserName = "test_user", Password = "test_user" };
			//var factory = new ConnectionFactory { HostName = "172.16.0.13", VirtualHost = "test", UserName = "test_user", Password = "test_user" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(exchange: "hello-exchange",
										type: "direct");

				channel.QueueDeclare(queue: "hello",
								durable: false,
								exclusive: false,
								autoDelete: false,
								arguments: null);
				channel.QueueBind(queue: "hello", exchange: "hello-exchange", "test");

				string message = "Hello World!";
				var body = Encoding.UTF8.GetBytes(message);
				//RabbitMQ.Client.IBasicProperties
				channel.BasicPublish(exchange: "hello-exchange",
									 routingKey: "test",
									 basicProperties: null,
									 body: body);
				Console.WriteLine(" [x] Sent {0}", message);
				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var _body = ea.Body.ToArray();
					var _message = Encoding.UTF8.GetString(body);
					Console.WriteLine(" [x] Received {0}", message);
				};
				channel.BasicConsume(queue: "hello",
								 autoAck: true,
								 consumer: consumer);

				Console.WriteLine(" Press [enter] to exit.");
				//Console.ReadLine();
			}


			//await bus.Publish(new TLBroadcastStats().CreateContext());

		}

	}

	public class HH : IMessageHandler<string>
	{
		public Task<object> Handle(IMessageContext<string> context)
		{
			return Task.FromResult<object>("ok");
		}
	}
}
