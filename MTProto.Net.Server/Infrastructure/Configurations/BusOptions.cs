using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Configurations
{
	public class BusOptions
	{
		public class RabbitMqSettings
		{
			public string Server { get; set; }
			public string VirtualHost { get; set; }
			public string UserName { get; set; }
			public string Password { get; set; }
			public string RequestQueName { get; set; }
			public string ReplyQueueName { get; set; }

			public RabbitMqSettings()
			{

			}
			public RabbitMqSettings Validate()
			{
				this.RequestQueName = string.IsNullOrWhiteSpace(this.RequestQueName) ? "rpc_requets" : this.RequestQueName;
				this.RequestQueName = string.IsNullOrWhiteSpace(this.RequestQueName) ? "rpc_replies" : this.RequestQueName;
				return this;

			}


		}
		public static BusOptions Instance = new BusOptions();

		public RabbitMqSettings RabbitMq { get; set; } = new RabbitMqSettings();

		public BusOptions Validate()
		{
			this.RabbitMq.Validate();
			return this;
		}


	}
}
