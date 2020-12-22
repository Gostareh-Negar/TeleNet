using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal.Internal
{
	class LocalDbConfig : LiteDbConfiguration
	{
		public LocalDbConfig()
		{
			this.ConnectionString = LibOptions.Current.GetLocalDbFileName();
		}
	}
	class LocalDb : LiteDbContext<LocalDbConfig>, ILocalDocumentStore
	{
		public LocalDb(LocalDbConfig config) : base(config) { }
	}
	class LocalDbRepository<TId, TEntity> : DocumentRepository<ILocalDocumentStore, TId, TEntity>, ILocalDocumentStoreRepository<TId, TEntity> where TEntity : class
	{
		public LocalDbRepository(ILocalDocumentStore context) : base(context)
		{

		}
	}

}
