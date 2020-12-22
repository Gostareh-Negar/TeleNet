using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Requests.Contacts;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbContactStore : LiteDbStoreBase<ContactData>, IContactStore
	{
		

		public async Task<IEnumerable<IContactData>> GetContacts(int userId, int offset = 0, int limit = 1000)
		{
			await Task.CompletedTask;
			var result = this.GetCollection()
				.Find(x => x.UserId == userId, offset, limit)
				.ToArray();
			return result;
		}

		public async Task<IContactData> ImportContact(int userId, int contactUserId, string firstName, string lastName, string phone, long clientId)
		{
			await Task.CompletedTask;
			ContactData result = null;
			result = this.GetCollection().Find(x => x.UserId == userId && x.ClientId == clientId, limit: 1)
				.FirstOrDefault();
			if (result == null)
			{
				result = new ContactData
				{
					UserId = userId,
					ImportedContactId = contactUserId,
					FirstName = firstName,
					LastName = lastName,
					Phone = phone,
					ClientId = clientId
				};
				this.GetCollection().Upsert(result);
			}
			else if (result.FirstName != firstName || result.LastName != lastName || result.ImportedContactId != contactUserId)
			{
				result.FirstName = firstName;
				result.LastName = lastName;
				result.ImportedContactId = contactUserId;
				this.GetCollection().Update(result);

			}
			return result;
		}


	}
}
