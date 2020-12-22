using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IUserManager
	{
		Task<IUser> GetUserBuAccessHash(long accessHash);
		Task<IUser> GetUserByMobilePhone(string phone);
		Task<IUser> GetUserById(int id);
		
		/// <summary>
		/// Creates a user with a valid phone and access hash.
		/// </summary>
		/// <param name="phone"></param>
		/// <param name="accessHash">User access hash this is  by defualt the auth_key_id of the sesssion.</param>
		/// <returns></returns>
		Task<IUser> CreateUser(string phone, long accessHash);
	}
}
