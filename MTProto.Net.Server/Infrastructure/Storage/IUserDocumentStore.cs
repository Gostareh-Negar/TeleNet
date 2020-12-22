using MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal;
using MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IUserDocumentStore : ILiteDbContext
	{
	}
	public interface IUserDocumentStoreRepository<TId, TEntity> : IDocumentRepository<TId, TEntity> where TEntity : class
	{

	}
}
