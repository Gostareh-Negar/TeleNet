using MTProto.NET.Schema.TL;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IUser : IUserPeer
	{
		IUserData UserData { get; }
		int Id { get; }
		string Phone { get; }

		Task AddUpdates(IStore store, TLUpdates updates);

		
	}
}
