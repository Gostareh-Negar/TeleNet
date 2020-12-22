using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.Layer72;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Services.Messages
{
	public interface IMessagingService
	{
		Task<IMessageData> SendPrivateChatMessage(int fromUserId, int toUserId, NET.Schema.Layer72.TLSendMessage sendMessage);
		Task<IEnumerable<IPrivateChatData>> GetPrivateChats(int userId, int offset = 0, int count = 1000);
		//Task<IEnumerable<IUserData>> GetUsersInPrivateCahts(int userId, int offset = 0, int count = 1000);
		
	}
	class MessagingService : IMessagingService
	{
		private readonly ILogger<MessagingService> logger;
		private readonly IStore store;

		public MessagingService(ILogger<MessagingService> logger, IStore store)
		{
			this.logger = logger;
			this.store = store;
		}

		public async Task<IEnumerable<IPrivateChatData>> GetPrivateChats(int userId, int offset = 0, int count = 1000)
		{
			await Task.CompletedTask;
			IEnumerable<IPrivateChatData> result = new List<IPrivateChatData>();

			try
			{
				using (var chatSTore = this.store.GetPrivateChatDataStore())
				{
					result = await chatSTore.GetByUserId(userId, offset, count);

				}

			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to 'GetPrivateChats: \r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}

		public async Task<IMessageData> SendPrivateChatMessage(int fromUserId, int toUserId, TLSendMessage sendMessage)
		{
			IMessageData result = null;
			try
			{
				using (var chatSTore = this.store.GetPrivateChatDataStore())
				{
					var chatData = await chatSTore.GetByParticipants(fromUserId, toUserId);
					if (chatData == null)
					{
						chatData = await chatSTore.Create(fromUserId, toUserId, "");
					}
					if (chatData == null)
					{
						throw new Exception("Failed to create chat!");
					}
					using (var message_store = this.store.GetMessageStore())
					{
						result = await message_store.CreatePrivateChatMessage(fromUserId, toUserId, sendMessage.Message, Extensions.ToTelegramDate(DateTime.UtcNow));
						if (result != null)
						{
							await MTServer.Bus.Publish(new Contracts.Messages.NewMessageCreated
							{
								FromUserId = result.FromId ?? 0,
								ToPeerId = result.ToPeerId,
								Text = result.Message,
								Id = result.Id,
								Date = result.Date
							});
						}

					}
				}

			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to 'SendPrivateMessage: \r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}
	}

}
