using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal.Internal
{
	class UserDbConfig : LiteDbConfiguration
	{
		public UserDbConfig()
		{
			this.ConnectionString = LibOptions.Current.GetUserDbFileName();
		}
	}
	class UserDb : LiteDbContext<UserDbConfig>, IUserDocumentStore
	{
		public UserDb(UserDbConfig config) : base(config) { }
	}
	class UserDbRepository<TId, TEntity> : DocumentRepository<IUserDocumentStore, TId, TEntity>, IUserDocumentStoreRepository<TId, TEntity> where TEntity : class
	{
		public UserDbRepository(IUserDocumentStore context) : base(context)
		{

		}
	}
}
