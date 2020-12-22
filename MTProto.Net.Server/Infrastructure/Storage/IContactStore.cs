using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IContactStore : IDisposable
	{
		Task<IEnumerable<IContactData>> GetContacts(int userId, int offset=0, int limit=1000);
		Task<IContactData> ImportContact(int userId, int contactUserId, string firstName, string lastName, string phone, long clientId);
		
	}
}
