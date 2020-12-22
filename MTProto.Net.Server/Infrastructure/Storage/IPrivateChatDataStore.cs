using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IPrivateChatDataStore : IDisposable
	{
		Task<IPrivateChatData> GetByParticipants(int userId1, int userId2);
		Task<IEnumerable<IPrivateChatData>> GetByUserId(int userId, int offset = 0, int limit = 1000);
		Task<IPrivateChatData> Create(int userId1, int userId2, string title);
	}
}
