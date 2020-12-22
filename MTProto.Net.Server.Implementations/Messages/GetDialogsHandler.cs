using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.Layer72;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Auth;
using MTProto.NET.Schema.TL.Requests.Auth;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Services.Contacts;
using MTProto.NET.Server.Services.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Messages
{
	/*
	 *	messages.getDialogs#a0ee3b73 flags:# exclude_pinned:flags.0?true folder_id:flags.1?int offset_date:date offset_id:int offset_peer:InputPeer limit:int hash:int = messages.Dialogs
	 *	Returns a set of dialogs as either a Dilogs or DialogSlice response.
	 *	It is normally used with an EmptyInputPeer.
	 *	In case of DialogSlice the response is only a slice of dialogs, the 'count' field will show the total number of dialogs so that the user will know
	 *	how many dialogs are there.
	 *	Terminology:
	 *		dialogs	:represents the conversations from your conversation history
	 *		chats	:represents the groups and channels corresponding to the conversations in your conversation history
	 *		messages :contains the last message sent to each conversation like you see in your list of conversations in your Telegram app
	 *		users	:contains the individual users with whom you have one-on-one chats with or who was the sender of the last message to one of your groups
	 *		
	 *		
	 *	
	 * Refernces:
	 *	1. https://towardsdatascience.com/introduction-to-the-telegram-api-b0cd220dbed2
	 *		This is a nice explanatory note on how GetDialogs is expected ro work.
	 *	2. https://tl.telethon.dev/methods/messages/get_dialogs.html
	 *		Telephon reference page.
	 */

	public class GetDialogsHandler : IProtoMessageHanddler<MTProto.NET.Schema.Layer72.TLGetDialogs>
	{
		private readonly ILogger logger;
		
		private readonly IMTSessionManager manager;
		private readonly IMessagingService messagingService;
		private readonly IContactService contactService;

		public GetDialogsHandler(ILogger<GetDialogsHandler> logger, IMTSessionManager manager,IMessagingService messagingService, IContactService contactService)
		{
			this.logger = logger;
			this.manager = manager;
			this.messagingService = messagingService;
			this.contactService = contactService;
		}

		public async Task<MTObject> Handle(IMessageContext<TLGetDialogs> context)
		{
			var result = await Task.FromResult<MTObject>(null);
			try
			{

				var dialogs = new TLVector<TLAbsDialog>();
				var users = new TLVector<TLAbsUser>();
				var chats = new TLVector<TLAbsChat>();
				var messages = new TLVector<TLAbsMessage>();
				var session = manager.GetSession(context.AuthKey());
				if (session == null)
					throw new Exception($"Session not found.");
				var user = await session.GetUser();
				var users_list = new List<int>();

				users.Add(new NET.Schema.Layer72.TLUser
				{
					Id = session.GetUserId(),
					FirstName = string.IsNullOrWhiteSpace(user?.UserData?.FirstName)? $"User {user.Id}": user.UserData.FirstName,
					Self = true
				}); ; 
				var dialog = new NET.Schema.Layer72.TLDialog
				{
					Peer = new TLPeerUser { UserId = session.GetUserId() },
					NotifySettings = new NET.Schema.Layer72.TLPeerNotifySettingsEmpty { }
				};
				dialogs.Add(dialog);
				var contacts = await contactService.GetContacts(session.GetUserId());
				

				foreach(var privateChat in await this.messagingService.GetPrivateChats(session.GetUserId(), 0, 1000))
				{
					if (!users_list.Contains(privateChat.UserId2))
					{
						users_list.Add(privateChat.UserId2);
					}
					dialog = new NET.Schema.Layer72.TLDialog
					{
						Peer = new TLPeerUser { UserId = privateChat.UserId2 },
						NotifySettings = new NET.Schema.Layer72.TLPeerNotifySettingsEmpty { }
					};
					dialogs.Add(dialog);
				}
				foreach(var contact in await contactService.GetContacts(session.GetUserId()))
				{
					if (!users_list.Contains(contact.ImportedContactId))
					{
						users_list.Add(contact.ImportedContactId);
					}
				}
				foreach(var chat_user in users_list)
				{
					var _user = await this.contactService.GetUser(chat_user);
					if (_user != null)
					{
						users.Add(new NET.Schema.Layer72.TLUser
						{
							Id = _user.Id,
							FirstName =_user.FirstName,
							LastName = _user.LastName,
							AccessHash = _user.Access_Hash,
							Phone = _user.Phone

						}); ;
					}
				}


				if (session.GetUserId()==1 && 1==0)
				{
					dialog = new NET.Schema.Layer72.TLDialog
					{
						Peer = new TLPeerUser { UserId = 2 },
						NotifySettings = new NET.Schema.Layer72.TLPeerNotifySettingsEmpty { }
					};
					dialogs.Add(dialog);
					users.Add(new NET.Schema.Layer72.TLUser
					{
						Id = 2,
						FirstName = $"User {2}",
						
					}); ;
				}
				if (session.GetUserId() == 2 && 1==0)
				{
					dialog = new NET.Schema.Layer72.TLDialog
					{
						Peer = new TLPeerUser { UserId = 1 },
						NotifySettings = new NET.Schema.Layer72.TLPeerNotifySettingsEmpty { }
					};
					dialogs.Add(dialog);
					users.Add(new NET.Schema.Layer72.TLUser
					{
						Id = 1,
						FirstName = $"User {1}",

					}); ;
				}
				result = new NET.Schema.TL.Messages.TLDialogs
				{
					Chats = chats,
					Dialogs = dialogs,
					Messages = new TLVector<TLAbsMessage>(),
					Users = users
				};


			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while trying to handle GetDialogs.:\r\n{err.GetBaseException().Message}");
				throw;

			}
			return result;

		}

		public async Task<MTObject> Handle_dep(IMessageContext<TLGetDialogs> context)
		{
			await Task.CompletedTask;
			var chats = new TLVector<TLAbsChat>();

			var chat = new NET.Schema.Layer72.TLChat
			{
				Title = "draft",
				Flags = 0,
				Id = 100,
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

			var users = new TLVector<TLAbsUser>()
			{
				
			};
			//users.Add(new TLUserEmpty { });
			users.Add(new NET.Schema.Layer72.TLUser
			{
				Id = 1,
				FirstName = "Ahmad"
			});
			users.Add(new NET.Schema.Layer72.TLUser
			{
				Id = 10,
				FirstName = "Babak"
			});
			users.Add(new NET.Schema.Layer72.TLUser
			{
				Id = 11,
				FirstName = "Zary"
			});
			var dialogs = new TLVector<TLAbsDialog>();
			var dialog = new NET.Schema.Layer72.TLDialog
			{
				Peer = new TLPeerUser { UserId = 1},
				NotifySettings =new NET.Schema.Layer72.TLPeerNotifySettingsEmpty { }
			};
			dialogs.Add(dialog);
			dialog = new NET.Schema.Layer72.TLDialog
			{
				Peer = new TLPeerUser { UserId = 10 },
				NotifySettings = new NET.Schema.Layer72.TLPeerNotifySettingsEmpty { }
			};
			dialogs.Add(dialog);
			dialog = new NET.Schema.Layer72.TLDialog
			{
				Peer = new TLPeerUser { UserId = 11 },
				NotifySettings = new NET.Schema.Layer72.TLPeerNotifySettingsEmpty { }
			};
			dialogs.Add(dialog);
			var result = new NET.Schema.TL.Messages.TLDialogs {
				Chats = chats,
				Dialogs = dialogs,
				Messages = new TLVector<TLAbsMessage>(),
				Users = users
			};

			//var bytes = result.ToByteArray();

			//List<uint> items = new List<uint>();
			//for(var i=0; i < bytes.Length; i = i + 4)
			//{
			//	items.Add(BitConverter.ToUInt32(bytes, i));
			//}
			//int[] bytesAsInts = Array.ConvertAll(bytes, c => (int)c);


			return result;


		}
	}
}
