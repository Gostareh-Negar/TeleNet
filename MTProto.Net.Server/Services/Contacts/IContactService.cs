using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Services.Contacts
{
	public interface IContactService
	{
		Task<IContactData> ImportContact(int userId, string firstName, string lastName, string phone, long clientId);
		Task<IEnumerable<IContactData>> GetContacts(int userId, int offset = 0, int limit = 1000);
		Task<IUserData> GetUser(int userId);


	}
	class ContactService : IContactService
	{
		private readonly ILogger<ContactService> logger;
		private readonly IStore store;

		public ContactService(ILogger<ContactService> logger, IStore store)
		{
			this.logger = logger;
			this.store = store;
		}

		public async Task<IEnumerable<IContactData>> GetContacts(int userId, int offset = 0, int limit = 1000)
		{
			IEnumerable<IContactData> result = new List<IContactData>();
			
			try
			{
			
				using (var _store = this.store.GetContactSTore())
				{
					result = await _store.GetContacts(userId);
					//using (var userStore = this.store.GetUserStore())
					//{
						
					//}

				}
			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to 'GetContacts':\r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}

		public async Task<IUserData> GetUser(int userId)
		{
			using(var userStore = this.store.GetUserStore())
			{
				return await userStore.GetUserById(userId);
			}
		}

		public async Task<IContactData> ImportContact(int userId, string firstName, string lastName, string phone, long clientId)
		{
			IContactData result = null;
			try
			{
				this.logger.LogDebug(
					$"Trying to import contact.{firstName} {lastName} {phone}");
				using (var _store = this.store.GetContactSTore())
				{
					using (var userStore = this.store.GetUserStore())
					{
						var helper = Extensions.GetMobilePhoneHelper(phone);
						if (!helper.IsValid)
							throw new Exception($"Invalid Mobile Phone: '{phone}'");
						phone = helper.AsTelegramFriendly;
						var importing_user = await userStore.GetUserById(userId);
						var imported_user = await userStore.GetUserByMobilePhone(phone);
						if (imported_user != null)
						{
							result = await _store.ImportContact(userId, imported_user.Id, imported_user.FirstName, imported_user.LastName, imported_user.Phone,clientId);
						}
						else
						{
							result = await _store.ImportContact(userId, 0, imported_user.FirstName, imported_user.LastName, imported_user.Phone,clientId);
						}
					}

				}
			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to 'ImportContact':\r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}
	}
}
