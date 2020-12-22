using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class Peer : IPeer
	{
		protected readonly IUserData data;

		public Peer(IUserData data)
		{
			this.data = data;
		}
	}
}
