using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IUserStore : IDisposable
	{
		Task<IUserData> GetUserByAccessHash(long access_hash);
		Task<IUserData> GetUserByMobilePhone(string phone);
		Task<IUserData> CreateUser(string phone, long accessHash);
		Task<IUserData> GetUserById(int id);

		Task<IUserData> Update(IUserData data);
		Task<IUserData> UpdateProfile(int userId, string firstName, string lastName, string about);
	}
}
