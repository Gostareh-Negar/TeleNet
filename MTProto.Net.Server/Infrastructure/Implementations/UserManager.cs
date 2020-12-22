using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class UserManager : IUserManager
	{
		private readonly IUserStore userStore;

		public UserManager(IUserStore userStore)
		{
			this.userStore = userStore;
		}

		public async Task<IUser> CreateUser(string phone, long accessHash)
		{
			IUser result = null;
			var helper = Extensions.GetMobilePhoneHelper(phone);
			if (!helper.IsValid)
				throw new Exception(
					$"Invalid Mobile Phone: '{phone}' is not a valid mobile phone number");
			var userData = await this.userStore.CreateUser(helper.AsTelegramFriendly, accessHash);
			if (userData != null)
				result = new User(userData);
			return result;
		}

		public Task<IUser> GetUserBuAccessHash(long accessHash)
		{
			throw new NotImplementedException();
		}

		public async Task<IUser> GetUserById(int id)
		{

			return new User(await userStore.GetUserById(id));
		}

		public async Task<IUser> GetUserByMobilePhone(string phone)
		{
			IUser result = null;
			var helper = Extensions.GetMobilePhoneHelper(phone);
			if (!helper.IsValid)
				throw new Exception(
					$"Invalid Mobile Phone: '{phone}' is not a valid mobile phone number");
			var userData = await this.userStore.GetUserByMobilePhone(helper.AsTelegramFriendly);
			if (userData != null)
				result = new User(userData);
			return result;
		}
	}
}
