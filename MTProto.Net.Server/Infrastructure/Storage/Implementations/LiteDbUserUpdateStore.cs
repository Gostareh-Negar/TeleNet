using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbUserUpdateStore : LiteDbStoreBase<UserUpdateData>, IUserUpdateStore
	{
		public async Task<IUserUpdateData> AddUpdate(IUserUpdateData data)
		{
			var _data = MTServer.Services.Mapper().Map<IUserUpdateData, UserUpdateData>(data);
			this.GetCollection().Upsert(_data);

			await Task.CompletedTask;
			return _data;
		}
	}
}
