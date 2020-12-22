using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MTProto.NET.Schema.Layer72;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class ChatManager : IChatManager
	{
		private readonly IChatStore chatStore;
		private readonly IChatParticipantStore chatParticipant;
		private readonly IMTObjectFactory objectFactory;
		private readonly IMTServiceProvider serviceProvider;

		public ChatManager(IChatStore chatStore, IChatParticipantStore chatParticipantStore, IMTObjectFactory objectFactory, IMTServiceProvider serviceProvider)
		{
			this.chatStore = chatStore;
			this.chatParticipant = chatParticipantStore;
			this.objectFactory = objectFactory;
			this.serviceProvider = serviceProvider;
		}
		public async Task<IChat> GetByChatId(int id)
		{
			return new Chat(await this.chatStore.GetById(id));
		}

		public async Task<IEnumerable<IMessage>> GetPrivateChatMessages(int fromUserId, int userPeerId)
		{
			using (var store = this.serviceProvider.GetService<IMessageStore>())
			{
				return (await store.GetPrivateMessages(fromUserId, userPeerId))
					.Select(x => this.objectFactory.CreateMessage(x))
					.ToArray();
			}
		}

		public async Task<IEnumerable<IChat>> GetUserChats(int userId, int offset = 0, int count = 30)
		{
			List<IChat> result = new List<IChat>();
			var participations = await this.chatParticipant.GetParticipantsByUserId(userId, offset, count);

			foreach (var p in participations)
			{
				result.Add(await this.GetByChatId(p.ChatId));
			}
			return result;
		}

		public async Task<IMessage> SendPrivateChatMessage(int fromUserId, int peersUserId, TLSendMessage message)
		{
			IMessage result = null;
			var data = this.objectFactory.CreateMessageData();
			data.ToPeerIdType = 0;
			data.ToPeerId = peersUserId;
			data.Message = message.Message;
			data.FromId = fromUserId;
			data.Date = MTServer.Services.Utils().ToTelegramDate(DateTime.UtcNow);
			using (var store = this.serviceProvider.GetService<IMessageStore>())
			{
				data = await store.CreateMessage(data);
			}
			result = this.objectFactory.CreateMessage(data);

			


			return result;

		}
	}
}
