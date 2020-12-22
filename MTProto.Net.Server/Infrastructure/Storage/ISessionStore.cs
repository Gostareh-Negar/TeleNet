using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface ISessionStore : IDisposable
	{
		SessionData Upsert(SessionData data);
		SessionData GetByClientNoonce(string clientNoonce, bool autoCreate = false);
		SessionData GetByAuthId(ulong authKeyId);
		SessionData GetByUserId(int userId);
	}
}
