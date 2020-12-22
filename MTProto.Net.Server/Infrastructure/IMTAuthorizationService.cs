
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure
{
	
	public interface IMTAuthorizationService
	{
		ulong m_pq();
		IAuthorizationSession GetOrCreateSession(string clientNonce);
	}
}
