using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Requests.Help;
using MTProto.NET.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.MT.Requests
{
	public class GetNearestDcHandler : IProtoMessageHanddler<TLGetNearestDc>
	{
		private readonly ILogger logger;
		private readonly IMTAuthorizationService authorizationService;
		private readonly IMTSessionManager manager;

		public GetNearestDcHandler(ILogger<GetNearestDcHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}

		public async Task<MTObject> Handle(IMessageContext<TLGetNearestDc> context)
		{
			await Task.CompletedTask;
			var result = new TLNearestDc
			{
				NearestDc = 1,
				ThisDc = 1,
				Country = "IR"
			};
			return result;
		}
	}
		
}
