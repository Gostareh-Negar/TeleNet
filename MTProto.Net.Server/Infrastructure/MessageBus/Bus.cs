using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Infrastructure.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MTProto.NET.Server.Infrastructure.Implementations
{

	class Bus : IHostedService, IBus
	{

		private readonly List<BusSubscription> subscriptions = new List<BusSubscription>();
		private readonly ILogger<Bus> logger;
		private readonly BusOptions options;
		private string uniqId;
		private RabbitMqHelper rabbit;

		public Bus(ILogger<Bus> logger, BusOptions options, RabbitMqHelper rabbitMqHelper)
		{

			this.subscriptions = new List<BusSubscription>();
			this.logger = logger;
			this.options = options.Validate();
			this.rabbit = rabbitMqHelper;
		}

		#region private methods
		private IEnumerable<BusSubscription> GetSubscriptions(string topic)
		{
			return this.subscriptions.Where(x => WildCardMatch(x.Topic, topic));
		}
		private async Task AddHandler<T>(object _handler) where T : MTObject
		{
			var handler = _handler as IProtoMessageHanddler<T>;
			
			await this.ProtoRegister<T>(true, cfg =>
			{
				cfg.Handler = async x =>
				{
					var arg = x;//as IMessageContext<T>;
					if (arg == null)
					{
						this.logger.LogWarning(
							"Null object being sent to ProtoMessageHandler. This maybe due to message routing issues.");
					}
					return await handler.Handle(arg);
				};
			});
		}

		private async Task AddHandler(IProtoMessageHanddler handler)
		{
			var type = handler.GetType();
			var protoHandlerType = type.GetInterfaces()
				.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IProtoMessageHanddler<>));
			if (protoHandlerType != null)
			{
				/// This is IProtoMessageHandler[T]
				/// subscribe to IMessageContext[t]
				var parameter = protoHandlerType.GetGenericArguments()[0];
				await Extensions.MakeGenericFunctionCall<object, Task>(this.AddHandler<MTObject>, parameter, handler);
			}
		}

		private async Task AddHandlerEx<T1>(object handler) where T1:class
		{
			var _handler = handler as IMessageHandler<T1,object>;
			await Subscribe<T1>(cfg => {

				cfg.Handler = x =>
				{
					return _handler.Handle(x);
				};
			});
		}
		private async Task AddHandlerEx(IMessageHandler handler)
		{
			var type = handler.GetType();
			var protoHandlerType = type.GetInterfaces()
				.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMessageHandler<,>));
			var parameter = protoHandlerType.GetGenericArguments()[0];
			await Extensions.MakeGenericFunctionCall<object, Task>(this.AddHandlerEx<object>, parameter, handler);

		}
		#endregion


		public static bool WildCardMatch(string pattern, string value)
		{
			if (pattern.Contains("*"))
			{

			}
			if (value == null || pattern == null)
				return false;
			var exp = "^" + Regex.Escape(pattern).Replace("\\?", ".").Replace("\\*", ".*") + "$";
			return Regex.IsMatch(value, exp);
		}

		public Task Publish(object message, string topic, CancellationToken cancellationToken)
		{
			if (message is null)
			{
				throw new ArgumentNullException(nameof(message));
			}
			if (string.IsNullOrWhiteSpace(topic))
			{
				topic = Extensions.GetTopic(message.GetType());
			}
			if (message as IMessageContext == null)
			{
				message = MessageContext.Create(message,topic);
			}

			return Task.WhenAll(this.GetSubscriptions(topic).Select(x => x.Handler(message as IMessageContext)));
		}

		
		public async Task<object> Send(object message, string topic = null, int timout = 30000, SendModes mode = SendModes.Both, CancellationToken cancellationToken = default)
		{
			object result = null;

			if (message is null)
			{
				throw new ArgumentNullException(nameof(message));
			}
			if (message as IMessageContext == null)
			{
				message = MessageContext.Create(message,topic);
			}
			var context = message as IMessageContext;
			if (string.IsNullOrWhiteSpace(topic))
			{
				topic = Extensions.GetTopic(message.GetType());
			}
			if (mode != SendModes.ExternalOnly)
			{
				foreach (var h in this.GetSubscriptions(topic).Where(x => x.IsRequestHandler))
				{
					try
					{
						result = await h.Handler(context).TimeoutAfter(30000, cancellationToken);
						return result;
					}
					catch { }
				}
			}
			if (mode != SendModes.InternalOnly && message as IMessageContext != null && this.rabbit.IsActive)
			{
				/// Try to send to remote 
				/// Note that we can only send message contexts...
				//var context = message as IMessageContext;
				try
				{
					var text = context.Serialize();
					result = await this.rabbit.Send(Encoding.UTF8.GetBytes(text));
				}
				catch { }
			}
			return result;
		}

		public async Task<BusSubscription> Subscribe(Action<BusSubscription> configure, CancellationToken cancellationToken = default)
		{
			await Task.CompletedTask;
			var subs = new BusSubscription();
			configure.Invoke(subs);
			lock (this.subscriptions)
			{
				this.subscriptions.Add(subs);
			}
			return subs;
		}
		public async Task<BusSubscription<T>> Subscribe<T>(Action<BusSubscription<T>> configure, string topic = null, CancellationToken cancellationToken = default) where T : class
		{
			await Task.CompletedTask;
			var subscription = new BusSubscription<T>();
			configure.Invoke(subscription);
			lock (this.subscriptions)
			{
				this.subscriptions.Add(subscription);
			}
			return subscription;
		}

		#region IHostedService

		private CancellationTokenSource stop;
		private Task exceutingTask;

		
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			stop = new CancellationTokenSource();
			var token = stop.Token;
			try
			{
				foreach (var handler in MTServer.Internal.GetService<IEnumerable<IProtoMessageHanddler>>())
				{
					await AddHandler(handler);
				};
				foreach (var handler in MTServer.Internal.GetService<IEnumerable<IMessageHandler>>())
				{
					await AddHandlerEx(handler);
				};
			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while trying to start MessageBus: { err} ");

			}
			await this.rabbit.Initialize(this, this.options);

			this.exceutingTask = Task.Run(async () =>
			{
				while (!token.IsCancellationRequested)
				{
					await Task.Delay(1 * 100);

				}

			});

			await Task.CompletedTask;
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			stop?.Cancel();
			if (exceutingTask != null)
				await this.exceutingTask;
			await Task.CompletedTask;
		}

		#endregion


	}
}
