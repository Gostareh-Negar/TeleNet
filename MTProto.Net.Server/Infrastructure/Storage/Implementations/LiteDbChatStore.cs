using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbChatStore : LiteDbStoreBase<ChatData>, IChatStore
	{
		public Task<IChatData> GetById(int chatId)
		{
			var result = this.GetCollection().FindById(chatId);
			return Task.FromResult<IChatData>(result);
		}
	}
}
