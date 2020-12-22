using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Auth;
using MTProto.NET.Schema.TL.Requests.Auth;
using MTProto.NET.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.MT.Requests
{
	/*
	 * 
	 */
	//auth.signIn#bcd51581 phone_number:string phone_code_hash:string phone_code:string = auth.Authorization;
	public class TLSignInHandler : IProtoMessageHanddler<TLSignIn>
	{
		private readonly ILogger logger;
		private readonly IMTSessionManager manager;

		public TLSignInHandler(ILogger<TLSignInHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}


		public async Task<MTObject> Handle(IMessageContext<TLSignIn> context)
		{
			await Task.CompletedTask;
			NET.Schema.TL.Auth.TLAuthorization result = null;
			try
			{

				var session = context.AuthKey().HasValue ? this.manager.GetSession(context.AuthKey().Value) : null;
				
				if (session == null)
					throw new Exception(
						$"Unexpected. Session not found. Auth_Key_Id:{context.AuthKey()}");

				/// Try to set the user for this session
				/// Note that it may create the user, or return the user
				/// that has already registered this mobile phone.
				/// 
				var user = await session.SetUser(context.Body.PhoneNumber);
				result = new NET.Schema.TL.Auth.TLAuthorization();
				result.Flags = 0;
				result.User = new MTProto.NET.Schema.Layer72.TLUser
				{
					Id = user.Id,
					FirstName = user.UserData.FirstName,
					LastName = user.UserData.LastName,
					AccessHash = user.UserData.Access_Hash,
					Phone = user.UserData.Phone,
					Self = true
				};
				this.logger.LogInformation(
					$"SignIn successfully completed. Phone: {context.Body.PhoneNumber}, User:{user}");
			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while trying to 'SignIn'. Phone{context.Body.PhoneNumber}. Error:\r\n:{err.GetBaseException().Message}");
				throw;
			}
			return result;
		}
	}
}
