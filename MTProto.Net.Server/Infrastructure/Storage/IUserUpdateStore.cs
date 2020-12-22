using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IUserUpdateStore:IDisposable
	{
		Task<IUserUpdateData> AddUpdate(IUserUpdateData data);
	}
}
