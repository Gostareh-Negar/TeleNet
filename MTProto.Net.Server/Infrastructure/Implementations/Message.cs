using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class Message : IMessage
	{
		public IMessageData Data { get; private set; }
		public Message(IMessageData data)
		{
			this.Data = data;
		}
	}
}
