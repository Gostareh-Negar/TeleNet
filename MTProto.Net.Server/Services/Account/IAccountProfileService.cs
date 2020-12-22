using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Contracts.Account;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Services.Account
{
	public interface IAccountProfileService
	{
		Task<IUserData> UpdateProfile(int accountId, string firstName, string lastName, string about);
	}
	class AccountProfileService : IAccountProfileService
	{
		private readonly IMTServiceProvider serviceProvider;
		private readonly ILogger<AccountProfileService> logger;

		public AccountProfileService(IMTServiceProvider serviceProvider, ILogger<AccountProfileService> logger)
		{
			this.serviceProvider = serviceProvider;
			this.logger = logger;
		}
		public async Task<IUserData> UpdateProfile(int accountId, string firstName, string lastName, string about)
		{
			IUserData result = null;
			try
			{
				using(var repo = this.serviceProvider.Store().GetUserStore())
				{
					result = await repo.UpdateProfile(accountId, firstName, lastName, about);
					if (result != null)
					{
						await serviceProvider.Bus().Publish(new ProfileChanged{
							AccountId = accountId,
							FirstName = result.FirstName,
							LastName = result.LastName,
							UserName = result.UserName
						});
					}

				}

			}
			catch (Exception err)
			{
				throw;
			}
			return result;
			
		}
	}
}
