using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IMessageStore:IDisposable
	{
		Task<IMessageData> CreateMessage(IMessageData data);
		Task<IEnumerable<IMessageData>> GetPrivateMessages(int fromId, int peerId, int offset = 0, int count = 1000);

		Task<IMessageData> CreatePrivateChatMessage(int fromId, int toUserId, string message, int date);
	}
}
