using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.Layer72;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Auth;
using MTProto.NET.Schema.TL.Messages;
using MTProto.NET.Schema.TL.Requests.Auth;
using MTProto.NET.Schema.TL.Requests.Messages;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Services.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Messages
{

	public class SendMessageHandler : IProtoMessageHanddler<MTProto.NET.Schema.Layer72.TLSendMessage>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;
		private readonly IMessagingService messagingService;

		public SendMessageHandler(ILogger<SendMessageHandler> logger, IMTSessionManager manager,IMessagingService messagingService)
		{
			this.logger = logger;
			this.manager = manager;
			this.messagingService = messagingService;
		}

		static int count;

		public async Task<MTObject> Handle(IMessageContext<NET.Schema.Layer72.TLSendMessage> context)
		{
			await Task.CompletedTask;
			TLUpdates result = null;
			try
			{
				result = new TLUpdates();
				result.Updates = new TLVector<TLAbsUpdate>();
				result.Users = new TLVector<TLAbsUser>();
				result.Chats = new TLVector<TLAbsChat>();
				var session = this.manager.GetSession(context.AuthKey());
				if (session == null)
					throw new Exception("Session Not Found.");
				var sendMessage = context.Body;
				if (sendMessage == null)
					throw new Exception(
						"SendMessage is NULL!");
				if (sendMessage.Peer == null)
					throw new ArgumentException(nameof(NET.Schema.Layer72.TLSendMessage.Peer));
				if (sendMessage.Peer is TLInputPeerUser)
				{
					//var sentMessage = await session.ChatManager.SendPrivateChatMessage(session.GetUserId(), (sendMessage.Peer as TLInputPeerUser).UserId, sendMessage);
					var sentMessage = await this.messagingService.SendPrivateChatMessage(session.GetUserId(), (sendMessage.Peer as TLInputPeerUser).UserId, sendMessage);
					//count++;
					//result.Seq = count;
					//result.Date = MTServer.Instance.Services.Utils().ToTelegramDate(DateTime.UtcNow);
					//result.Updates.Add(new TLUpdateNewMessage
					//{
					//	Pts = count,
					//	PtsCount = 1,

					//	Message = new MTProto.NET.Schema.Layer72.TLMessage
					//	{
					//		FromId = session.GetUserId(),
					//		Id = sentMessage.Data.Id,
					//		Message = sentMessage.Data.Message,
					//		ToId = new TLPeerUser
					//		{
					//			UserId = sentMessage.Data.ToPeerId
					//		}
					//	}
					//});
					//await session.UpdateManager.SendUpdate(result);
					//result = new TLUpdates();
					//result.Updates = new TLVector<TLAbsUpdate>();
					//result.Users = new TLVector<TLAbsUser>();
					//result.Chats = new TLVector<TLAbsChat>();
				}
				else
				{
					throw new NotImplementedException();

				}




			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while trying to handle SendMessageRequest. Request:{context}, Error:\r\n{err.GetBaseException().Message}");
				throw;
			}

			//result.Updates.Add(new TLUpdateNewMessage
			//{

			//	Message = new MTProto.NET.Schema.Layer72.TLMessage
			//	{
			//		Message = "hi there",
			//		ToId = new TLPeerUser
			//		{
			//			UserId = 12,
			//		}
			//	}
			//});

			return result;
		}
	}
}
