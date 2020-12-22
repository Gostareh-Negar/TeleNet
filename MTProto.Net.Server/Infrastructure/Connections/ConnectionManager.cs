using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Connections
{
	class ConnectionManager : IConnectionManager
	{
		private readonly ILogger<ConnectionManager> logger;
		private readonly IConnectionStore connectionStore;

		public ConnectionManager(ILogger<ConnectionManager> logger, IConnectionStore connectionStore)
		{
			this.logger = logger;
			this.connectionStore = connectionStore;
		}
	}
}
