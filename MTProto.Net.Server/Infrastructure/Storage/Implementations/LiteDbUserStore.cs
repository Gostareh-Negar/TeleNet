using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class LiteDbUserStore : LiteDbStoreBase<UserData>, IUserStore
	{
		public override void OnCollectionCreated(ILiteCollection<UserData> collection)
		{
			collection.EnsureIndex(x => x.Access_Hash, false);
			collection.EnsureIndex(x => x.Phone, true);
		}
		public Task<IUserData> GetUserByAccessHash(long access_hash)
		{
			return Task.FromResult<IUserData>(this.GetCollection()
				.Find(x => x.Access_Hash == access_hash, limit: 1)
				.FirstOrDefault());
		}

		public Task<IUserData> GetUserByMobilePhone(string phone)
		{
			var result = this.GetCollection()
				.Find(x => x.Phone == phone, limit: 1)
				.FirstOrDefault();

			return Task.FromResult<IUserData>(result);

		}

		public async Task<IUserData> CreateUser(string phone, long accessHash)
		{
			var existing = (await this.GetUserByMobilePhone(phone));// ??
								//(await this.GetUserByAccessHash(accessHash));

			if (existing != null)
				throw new Exception(
					$"User already exists. Another user already exists with this phone ('{phone}') or hash ('{accessHash}')");
			var result = new UserData
			{
				Phone = phone,
				Access_Hash = accessHash
			};
			this.GetCollection().Upsert(result);
			return (result);
		}

		public async Task<IUserData> GetUserById(int id)
		{
			await Task.CompletedTask;
			return this.GetCollection().Find(x => x.Id == id).FirstOrDefault();
		}

		public async Task<IUserData> Update(IUserData data)
		{
			await Task.CompletedTask;
			var userData = MTServer.Services.Mapper().Map<IUserData, UserData>(data);
			this.GetCollection().Upsert(userData);
			return userData;
		}

		public async Task<IUserData> UpdateProfile(int userId, string firstName, string lastName, string about)
		{
			var data = await this.GetUserById(userId) as UserData;
			if (data != null)
			{
				data.FirstName = firstName;
				data.LastName = lastName;
				data.About = about;
				this.GetCollection().Update(data);
				return data;
			}
			return null;

		}
	}
}
