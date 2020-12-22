using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class Chat : IChat
	{
		private readonly IChatData chatData;

		public Chat(IChatData chatData)
		{
			this.chatData = chatData;
		}
	}
}
