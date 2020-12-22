using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.MT.Requests
{
	public class PingHandler : IProtoMessageHanddler<MTPing>
	{
		private readonly ILogger logger;
		private readonly IMTAuthorizationService authorizationService;
		private readonly IMTSessionManager manager;

		public PingHandler(ILogger<PingHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}

		public async Task<MTObject> Handle(IMessageContext<MTPing> context)
		{
			await Task.CompletedTask;
			var result = new MTPong
			{
				MsgId = (long) context.container_msg_id(null).Value,
				PingId = context.Body.PingId

			};
			this.logger.LogDebug("Ping....");
			return result;
		}

		
	}
}
