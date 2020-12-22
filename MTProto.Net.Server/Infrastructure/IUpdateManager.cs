using MTProto.NET.Schema.TL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IUpdateManager
	{
		Task SendUpdate(TLUpdates update);
		Task<TLUpdates> GetPedningUpdate(int userId);


	}
}
