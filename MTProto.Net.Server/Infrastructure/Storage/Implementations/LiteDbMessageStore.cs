using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbMessageStore : LiteDbStoreBase<MessageData>, IMessageStore
	{

		public override void OnCollectionCreated(ILiteCollection<MessageData> collection)
		{
			base.OnCollectionCreated(collection);
		}
		public async Task<IMessageData> CreateMessage(IMessageData data)
		{
			await Task.CompletedTask;
			MessageData messageData = MTServer.Services.Mapper().Map<IMessageData, MessageData>(data);
			messageData.ToPeerIdType = 0;
			if (messageData.Date == 0)
				messageData.Date = Extensions.ToTelegramDate(DateTime.UtcNow);
			this.GetCollection().Upsert(messageData);
			await MTServer.Bus.Publish(new Contracts.Messages.NewMessageCreated
			{
				FromUserId = data.FromId ?? 0,
				ToPeerId = data.ToPeerId,
				Text = data.Message,
				Id = messageData.Id,
				Date = data.Date
			});
			return messageData;
		}

		public async Task<IEnumerable<IMessageData>> GetPrivateMessages(int fromId, int peerId, int offset = 0, int count = 1000)
		{
			await Task.CompletedTask;
			return this.GetCollection()
				.Find(x => ((x.FromId == fromId && x.ToPeerId == peerId) || (x.FromId == peerId && x.ToPeerId == fromId)) && x.ToPeerIdType == 0, skip: offset, limit: count)
				.OrderBy(x => x.Date);

		}

		public async Task<IMessageData> CreatePrivateChatMessage(int fromId, int toUserId, string message, int date)
		{
			await Task.CompletedTask;
			var messageData = new MessageData
			{
				FromId = fromId,
				ToPeerId = toUserId,
				ToPeerIdType = 0,
				Message = message,
				Date = date == 0 ? Extensions.ToTelegramDate(DateTime.UtcNow) : date
			};
			this.GetCollection().Upsert(messageData);
			return messageData;

		}
	}
}
