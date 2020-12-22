using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL.Requests.Auth;
using MTProto.NET.Schema.TL.Requests.Updates;
using MTProto.NET.Schema.TL.Updates;
using MTProto.NET.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Updates
{
	/*
	 * Fetching State :https://core.telegram.org/api/updates#fetching-state
	 * TLGetState Api Page :https://core.telegram.org/method/updates.getState
	 * 
	 */

	public class GetStateHandler : IProtoMessageHanddler<TLGetState>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;

		public GetStateHandler(ILogger<GetStateHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}

		public async Task<MTObject> Handle(IMessageContext<TLGetState> context)
		{
			MTObject result = null;
			var session = manager
				.GetSession(context.AuthKey());
			var user = await session.GetUser();
			try
			{
				
				result = new TLState
				{
					Date = MTServer.Instance.Services.Utils().ToTelegramDate(DateTime.UtcNow),
					Pts = user.UserData.Pts,
					Qts = 0,
					Seq =user.UserData.Seq,
					UnreadCount = 0
				};
			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while trying to handle GetState: \r\n{err.GetBaseException().Message}");
			}
			return result;
		}
	}
}
