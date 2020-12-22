using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IChatParticipantStore
	{
		Task<IEnumerable<IChatParticipant>> GetParticipantsByUserId(int userId, int offset = 0, int count = 30);
	}
}
