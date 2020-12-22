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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MTProto.NET.Server.Implementations.Messages
{
	/*
	 * 
	 */
	/// <summary>
	/// 
	/// </summary>

	public class GetHistoryHandler : IProtoMessageHanddler<TLGetHistory>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;

		public GetHistoryHandler(ILogger<GetHistoryHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}

		public async Task<MTObject> Handle(IMessageContext<TLGetHistory> context)
		{
			await Task.CompletedTask;
			MTObject result = null;
			try
			{
				var chats = new TLVector<TLAbsChat>();
				var messages = new TLVector<TLAbsMessage>();
				var users = new TLVector<TLAbsUser>();
				var session = this.manager.GetSession(context.AuthKey());
				if (session == null)
					throw new Exception("Session not found.");
				var request = context.Body;
				if (request == null)
					throw new ArgumentException(nameof(TLGetHistory));
				if (request.Peer == null)
					throw new ArgumentException(nameof(TLGetHistory.Peer));
				if (request.Peer is TLInputPeerUser)
				{
					var _messages = (await session.ChatManager.GetPrivateChatMessages(session.GetUserId(), (request.Peer as TLInputPeerUser).UserId))
						.OrderByDescending(x => x.Data.Date);
					_messages.Select(x => new NET.Schema.Layer72.TLMessage
					{
						Message = x.Data.Message,
						FromId = x.Data.FromId,
						Id = x.Data.Id,
						Date = x.Data.Date,
						ToId = new TLPeerUser
						{
							UserId = x.Data.ToPeerId,
						}
					}).ToList().ForEach(x => messages.Add(x));

					foreach (var m in _messages)
					{
						if (m.Data.FromId.HasValue && m.Data.FromId!= session.GetUserId() && !users.Any(x=>(x as MTProto.NET.Schema.Layer72.TLUser)?.Id==m.Data.FromId ))
						{
							using (var user_store = session.Services.Store().GetUserStore())
							{
								var __user = await user_store.GetUserById(m.Data.FromId.Value);
								if (__user != null)
								{
									users.Add(new MTProto.NET.Schema.Layer72.TLUser { 
										Id = __user.Id,
										FirstName = string.IsNullOrWhiteSpace(__user.FirstName)? $"{__user.Id}":__user.FirstName,
										Phone = __user.Phone,
									});
								}
							}
						}
					}

				}
				else
				{
					throw new NotImplementedException();
				}
				//users.Add(new NET.Schema.Layer72.TLUser
				//{
				//	Id = session.GetUserId(),
				//	FirstName = "Babak",
				//	Self = true
				//});
				result = new TLMessages()
				{
					Messages = messages,// new TLVector<TLAbsMessage> { },
					Chats = chats,
					Users = users

				};

			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while trying to handle GetHistoryRequest. Error:\r\n{err.GetBaseException().Message}");
				throw;
			}
			return result;
		}

		public async Task<MTObject> Handle_Deperacted(IMessageContext<TLGetHistory> context)
		{
			await Task.CompletedTask;
			var chats = new TLVector<TLAbsChat>();
			var chat = new NET.Schema.Layer72.TLChat
			{
				Title = "draft12",
				Flags = 0,
				Id = 101,
				admin = true,

				Photo = new TLChatPhotoEmpty
				{

				}
				//Photo = new TLChatPhoto { 
				//	Flags = 0,
				//	HasVideo = false,
				//	PhotoBig = new TLFileLocationToBeDeprecated { },
				//	PhotoSmall = new TLFileLocationToBeDeprecated { }
				//}

			};
			chats.Add(chat);
			var message = new MTProto.NET.Schema.Layer72.TLMessage
			{
				Message = "hi therexxx",
				ToId = new TLPeerUser
				{
					UserId = 12,
				}
			};
			var messages = new TLVector<TLAbsMessage> { };
			messages.Add(message);
			var users = new TLVector<TLAbsUser>()
			{

			};
			//users.Add(new TLUserEmpty { });
			users.Add(new NET.Schema.Layer72.TLUser
			{
				Id = 1,
				FirstName = "Ahmad"
			});
			var result = new TLMessages()
			{
				Messages = messages,// new TLVector<TLAbsMessage> { },
				Chats = chats,
				Users = users

			};

			return result;
		}
	}
}
