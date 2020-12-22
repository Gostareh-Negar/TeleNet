using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class MTUtilityServices : IMTUtilityService
	{
		public IMTServiceProvider ServiceProvider { get; set; }

		//public MTUtilityServices(IMTServiceProvider provider)
		public MTUtilityServices() : this(null) { }
		public MTUtilityServices(IMTServiceProvider provider)
		{
			this.ServiceProvider = provider ?? MTServer.Services;
		}
	}
}
