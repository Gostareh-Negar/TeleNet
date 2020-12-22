using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL.Messages;
using MTProto.NET.Schema.TL.Requests.Channels;
using MTProto.NET.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Messages
{

	public class ReadHistoryHandler : IProtoMessageHanddler<TLReadHistory>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;

		public ReadHistoryHandler(ILogger<ReadHistoryHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}

		public async Task<MTObject> Handle(IMessageContext<TLReadHistory> context)
		{
			await Task.CompletedTask;
			var result = new TLAffectedMessages();
			try
			{

			}
			catch (Exception err)
			{

			}
			return result;
		}
	}
}
