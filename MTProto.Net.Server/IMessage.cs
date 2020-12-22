using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server
{
	public interface IMessage
	{
		IMessageData Data { get; }
	}
}
