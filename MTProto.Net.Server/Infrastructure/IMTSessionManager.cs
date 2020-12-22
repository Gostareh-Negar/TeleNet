using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IMTSessionManager
	{
		Task<IMTSession> KeepAlive(ulong sessionId, ulong auth_key);
		IAuthorizationSession GetOrCreateAuthorizationSession(string clientNonce);
		IAuthorizationSession GetOrCreateAuthorizationSession(BigInteger clientNonce);
		IMTSession GetSession(ulong? AuthKeyId);
		IMTSession GetSessionByUserId(int userId);
		//void KeepAlive(ulong AuthKeyId, ulong SessionId);


	}
}
