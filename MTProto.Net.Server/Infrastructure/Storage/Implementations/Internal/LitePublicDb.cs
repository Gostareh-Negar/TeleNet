using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal.Internal
{
	class PublicDbConfig : LiteDbConfiguration
	{
		public PublicDbConfig()
		{
			this.ConnectionString = LibOptions.Current.GetPublicDbFileName();
		}
	}
	class PublicDb : LiteDbContext<PublicDbConfig>, IPublicDocumentStore
	{
		public PublicDb(PublicDbConfig config) : base(config) { }
	}
	class PublicDbRepository<TId, TEntity> : DocumentRepository<IPublicDocumentStore, TId, TEntity>, IPublicDbRepository<TId, TEntity> where TEntity : class
	{
		public PublicDbRepository(IPublicDocumentStore context) : base(context)
		{

		}
	}
}
