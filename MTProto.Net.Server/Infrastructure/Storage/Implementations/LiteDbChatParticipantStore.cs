using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbChatParticipantStore : LiteDbStoreBase<ChatParticipantData>, IChatParticipantStore
	{
		public override void OnCollectionCreated(ILiteCollection<ChatParticipantData> collection)
		{
			base.OnCollectionCreated(collection);
		}
		public async Task<IEnumerable<IChatParticipant>> GetParticipantsByUserId(int userId, int offset = 0, int count = 30)
		{
			await Task.CompletedTask;
			var result = this.GetCollection()
				.Find(x => x.UserId == userId, skip: offset, limit: count)
				.ToArray();
			return result;
		}
	}
}
