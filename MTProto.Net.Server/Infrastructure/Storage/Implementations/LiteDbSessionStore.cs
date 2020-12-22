using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbSessionStore : LiteDbStoreBase<SessionData>, ISessionStore
	{
		protected override ILiteCollection<SessionData> GetCollection(bool refersh = false)
		{
			var result = base.GetCollection();
			result.EnsureIndex(x => x.ClientNonce);
			return result;
		}
		public SessionData GetByClientNoonce(string clientNoonce, bool autoCreate = false)
		{
			var result = this.GetCollection().Find(x => x.ClientNonce == clientNoonce, limit: 1).FirstOrDefault();
			if (result==null && autoCreate)
			{
				result = new SessionData
				{
					ClientNonce = clientNoonce
				};
				result = Upsert(result);
			}
			return result;
		}

		public SessionData Upsert(SessionData data)
		{
			data.CreatedOn = data.CreatedOn ?? DateTime.UtcNow;
			this.GetCollection().Upsert(data);
			return data;
		}

		public SessionData GetByAuthId(ulong authKeyId)
		{
			var items = this.GetCollection().FindAll().ToList();
			return this.GetCollection().Find(x => x.AuthKeyId == authKeyId)
				.FirstOrDefault();
		}

		public SessionData GetByUserId(int userId)
		{
			return this.GetCollection().Find(x => x.UserId == userId).FirstOrDefault();
		}
	}
}
