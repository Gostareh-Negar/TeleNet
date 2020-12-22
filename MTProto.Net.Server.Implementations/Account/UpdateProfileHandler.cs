using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Requests.Account;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Services.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Account
{
	/* 
	 *
	 * References:
	 *	1. https://core.telegram.org/method/account.updateProfile
	 */
	public class UpdateProfileHandler : IProtoMessageHanddler<TLUpdateProfile>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;
		private readonly IAccountProfileService profileService;

		public UpdateProfileHandler(ILogger<UpdateProfileHandler> logger, IMTSessionManager manager, IAccountProfileService profileService)
		{
			this.logger = logger;
			this.manager = manager;
			this.profileService = profileService;
		}

		public async Task<MTObject> Handle(IMessageContext<TLUpdateProfile> context)
		{
			MTObject result = null;
			try
			{

				var session = manager.GetSession(context.AuthKey());
				if (session == null)
					throw new Exception("Session not found!");
				var userId = session.GetUserId();
				var profile = context.Body;
				var data = await this.profileService.UpdateProfile(userId, profile.FirstName, profile.LastName, profile.About);
				if (data != null)
				{
					result = new MTProto.NET.Schema.Layer72.TLUser
					{
						FirstName = data.FirstName,
						LastName = data.LastName,
						Id = data.Id,
						Self = true
					};
				}
				else
				{
					result = new TLUserEmpty
					{

					};
				}



			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to handle 'UpdateProfile': \r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}
	}
}
