using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public class ConnectionData
	{
		public Guid Id { get; set; }
		public string ConnectionId { get; set; }
		public Dictionary<string,string> Headers { get; set; }

	}
}
