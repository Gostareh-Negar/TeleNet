using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbConnectionStore : LiteDbStoreBase, IConnectionStore
	{
		public LiteDbConnectionStore()
		{

		}
		private LiteDB.ILiteCollection<ConnectionData> GetCollection()
		{
			var collection = this.GetDatabase().GetCollection<ConnectionData>();
			collection.EnsureIndex(x => x.ConnectionId);
			return collection;

		}
		public ConnectionData GetConnectionId(string connectionId)
		{
			var collection = this.GetDatabase().GetCollection<ConnectionData>();
			collection.EnsureIndex(x => x.ConnectionId);
			return collection.Find(x => x.ConnectionId == connectionId).FirstOrDefault();
		}

		public ConnectionData Upsert(ConnectionData connection)
		{
			this.GetCollection().Upsert(connection);
			return connection;
		}
	}
}
