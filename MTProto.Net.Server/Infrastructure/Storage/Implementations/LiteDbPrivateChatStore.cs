using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbPrivateChatStore : LiteDbStoreBase<PrivateChatData>, IPrivateChatDataStore
	{
		public async Task<IPrivateChatData> Create(int userId1, int userId2, string title)
		{
			var result = await GetByParticipants(userId1, userId2) as PrivateChatData;
			if (result == null)
			{
				result = new PrivateChatData
				{
					UserId1 = userId1,
					UserId2 = userId2
				};
				this.GetCollection().Upsert(result);
			}
			return result;

		}

		public async Task<IPrivateChatData> GetByParticipants(int userId1, int userId2)
		{
			await Task.CompletedTask;
			var result = this.GetCollection()
				//.Find(x => (x.UserId1 == userId1 && x.UserId2 == userId2) || (x.UserId1 == userId2 && x.UserId2 == userId1), limit: 1)
				.Find(x => (x.UserId1 == userId1 && x.UserId2 == userId2), limit: 1)
				.FirstOrDefault();
			return result;
		}

		

		public async Task<IEnumerable<IPrivateChatData>> GetByUserId(int userId, int offset = 0, int limit = 1000)
		{
			await Task.CompletedTask;
			var result = this.GetCollection()
				.Find(x => x.UserId1 == userId, skip: offset, limit: limit)
				.ToArray();
			return result;
		}
	}
}
