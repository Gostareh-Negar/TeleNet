using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Contracts.Authorization
{
	public class NewSessionCreated
	{
		public ulong SessionId { get; set; }
		public ulong AuthKeyId { get; set; }
	}
}
