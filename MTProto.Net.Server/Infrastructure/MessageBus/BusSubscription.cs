using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Implementations
{

	public class BusSubscription
	{
		public string Topic { get;  set; }
		public bool IsRequestHandler { get; set; }
		public MessageHandler Handler { get; set; }
		public bool Durable { get; set; }

		public BusSubscription SetTopicByType(Type type)
		{
			this.Topic = Extensions.GetTopic(type);
			return this;
		}
	}
	public class BusSubscription<T> : BusSubscription where T : class
	{
		private MessageHandler<T> _handler;
		public BusSubscription()
		{
			//this.Topic = Extensions.GetTopic(typeof(T));
			SetTopicByType(typeof(T));
		}
		public new MessageHandler<T> Handler
		{
			get { return this._handler; }
			set
			{
				this._handler = value;
				base.Handler = x => this._handler(x as IMessageContext<T>);
			}
		}
	}
	public class BusNotificationSubscription<T> : BusSubscription where T : class
	{
		private NotificationHandler<T> _handler;
		public BusNotificationSubscription()
		{
			this.Topic = Extensions.GetTopic(typeof(T));
		}
		public new NotificationHandler<T> Handler
		{
			get 
			{ 
				return this._handler; 
			}
			set
			{
				this._handler = value;
				base.Handler = x =>
				{
					this._handler(x as IMessageContext<T>);
					return Task.FromResult<object>(true);
				};
					
			}
		}
	}

}
