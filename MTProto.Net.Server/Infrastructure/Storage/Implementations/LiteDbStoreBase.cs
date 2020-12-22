using LiteDB;
using MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbStoreBase : IDisposable
	{
		private string connectionString;
		LiteDB.LiteDatabase db;
		public LiteDbStoreBase(string connectionString = null)
		{
			this.connectionString = connectionString ?? LibOptions.Current.GetPublicDbFileName();
			this.connectionString = $"FileName={LibOptions.Current.GetPublicDbFileName()}; Connection=shared";
		}

		public void Dispose()
		{
			this.db?.Dispose();
		}



		protected virtual LiteDB.LiteDatabase GetDatabase(bool refersh = false)
		{
			if (this.db == null || refersh)
			{
				this.db = new LiteDB.LiteDatabase(connectionString);
			}
			return this.db;
		}
	}
	class LiteDbStoreBase<T> : LiteDbStoreBase where T : class
	{
		private ILiteCollection<T> collection;
		protected virtual LiteDB.ILiteCollection<T> GetCollection(bool refersh = false)
		{
			if (this.collection == null || refersh)
			{

				this.collection = this.GetDatabase(refersh).GetCollection<T>();
				this.OnCollectionCreated(collection);
			}
			return this.collection;
		}
		public virtual void OnCollectionCreated(LiteDB.ILiteCollection<T> collection)
		{

		}
	}

}
