using Microsoft.Extensions.Logging;
using MTProto.NET.Serializers;
using MTProto.NET.Server.Infrastructure.Helpers;
using MTProto.NET.Server.Infrastructure.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Server.Infrastructure
{
	public class MessagePacket
	{
		public Dictionary<string, string> Headers { get; set; }
		public Byte[] Data { get; set; }
		public string Type { get; set; }
	}
	public class MessageContext : IMessageContext
	{
		private ConcurrentDictionary<string, string> headers = new ConcurrentDictionary<string, string>();
		private ConcurrentDictionary<string, object> cache = new ConcurrentDictionary<string, object>();
		protected object message;

		private IMTServiceProvider serviceProvider;

		[JsonProperty]
		public IDictionary<string, string> Headers { get => this.headers; set => this.headers = new ConcurrentDictionary<string, string>(value); }

		[JsonIgnore]
		public IDictionary<string, object> Cache => this.cache;

		[JsonIgnore]
		public object Body => this.message;

		[JsonIgnore]
		public IMTServiceProvider Services => this.serviceProvider;

		public string Topic { get;  set; }

		protected MessageContext(object message, string topic = null, IDictionary<string, string> headers = null, IDictionary<string, object> cache = null, IMTServiceProvider serviceProvider = null)
		{
			this.message = message;
			this.headers = new ConcurrentDictionary<string, string>(headers ?? new Dictionary<string, string>());
			this.cache = new ConcurrentDictionary<string, object>(cache ?? new Dictionary<string, object>());
			this.serviceProvider = serviceProvider ?? MTServer.Services;

			this.Topic = topic ?? (message == null ? null : Extensions.GetTopic(message.GetType()));

		}
		public IMessageContext<T> Cast<T>() where T : class
		{
			if (message != null && !typeof(T).IsAssignableFrom(this.message.GetType()))
			{
				throw new Exception($"Invalid message cast. Cannot cast {this.message.GetType().Name} to {typeof(T).Name}");
			}
			return MessageContext<T>.Create(this.message as T,this.Topic, this.headers, this.cache, this.serviceProvider);
		}

		public IMessageContext Cast(Type type)
		{
			type = type ?? this.message?.GetType() ?? typeof(object);
			return Extensions.MakeGenericFunctionCall<object>(this.Cast<object>, type) as IMessageContext;
		}
		public static IMessageContext Create(object message, string topic = null, IDictionary<string, string> headers = null, IDictionary<string, object> cache = null, IMTServiceProvider serviceProvider = null)
		{
			return new MessageContext(message, topic, headers, cache, serviceProvider).Cast(null);
		}
		public static IMessageContext Create(string packet)
		{
			MessagePacket _packet = JsonConvert.DeserializeObject<MessagePacket>(packet);
			object message = null;
			if (_packet.Data != null && _packet.Data.Length > 1)
			{
				message = _packet.Type == typeof(MTObject).Name
					? MTObjectSerializer.DeserializeEx(_packet.Data)
					: JsonConvert.DeserializeObject(Encoding.UTF8.GetString(_packet.Data), Type.GetType(_packet.Type));
			}
			return MessageContext.Create(message, null, _packet.Headers);
		}
	}
	public class MessageContext<T> : MessageContext, IMessageContext<T> where T : class
	{
		public T Body => this.message as T;

		protected MessageContext(T message, string topic=null, IDictionary<string, string> headers = null, IDictionary<string, object> cache = null, IMTServiceProvider serviceProvider = null) :
			base(message, topic, headers, cache, serviceProvider)
		{


		}
		public static IMessageContext<T> Create(T message, string topic=null, IDictionary<string, string> headers = null, IDictionary<string, object> cache = null, IMTServiceProvider serviceProvider = null)
		{
			return new MessageContext<T>(message,topic, headers, cache, serviceProvider);
		}
	}

	public static class MessageContextExtensions
	{
		public static Type MessageType(this IMessageContext context)
		{

			return context.Body?.GetType();
		}
		public static ILogger GetLogger(this IMessageContext context)
		{
			return context.Cache.GetOrAddValue<ILogger>(() => context.Services.GetService<ILogger<MessageContext>>());

		}

		public static string Serialize(this IMessageContext context)
		{
			var result = "";

			var packet = new MessagePacket();
			packet.Headers = new Dictionary<string, string>(context.Headers);
			packet.Data = new byte[] { };

			if (context.Body != null)
			{
				if (context.Body as MTObject != null)
				{
					packet.Data = MTObjectSerializer.SerializeEx(context.Body as MTObject);
					packet.Type = typeof(MTObject).Name;
				}
				else
				{
					packet.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(context.Body));
					packet.Type = context.Body.GetType().AssemblyQualifiedName;
				}
			}
			return JsonConvert.SerializeObject(packet);
		}
		public static IMessageContext DeserializeMessage(this IMTServiceProvider services, string packet)
		{
			IMessageContext result = null;
			MessagePacket _packet = JsonConvert.DeserializeObject<MessagePacket>(packet);
			object message = null;
			if (_packet.Data != null && _packet.Data.Length > 1)
			{
				message = _packet.Type == typeof(MTObject).Name
					? MTObjectSerializer.DeserializeEx(_packet.Data)
					: JsonConvert.DeserializeObject(Encoding.UTF8.GetString(_packet.Data), Type.GetType(_packet.Type));
			}
			result = MessageContext.Create(message,null, _packet.Headers, serviceProvider: services);
			return result;

		}
	}



}
