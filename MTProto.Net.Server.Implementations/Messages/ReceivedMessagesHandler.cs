using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Requests.Messages;
using MTProto.NET.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Messages
{

	/*
	 * 
	 * References :
	 *	1. https://core.telegram.org/method/messages.receivedMessages
	 */

	public class ReceivedMessagesHandler : IProtoMessageHanddler<TLReceivedMessages>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;

		public ReceivedMessagesHandler(ILogger<ReceivedMessagesHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}

		public async Task<MTObject> Handle(IMessageContext<TLReceivedMessages> context)
		{
			await Task.CompletedTask;

			var result = new TLVector<TLReceivedNotifyMessage>();

			return result;
		}
	}
}
