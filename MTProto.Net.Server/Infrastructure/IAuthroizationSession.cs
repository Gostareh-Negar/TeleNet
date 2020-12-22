using MTProto.NET.Server.Infrastructure.Storage;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IAuthorizationSession : IMTSession
	{
		SessionData Data { get; }
		ulong m_pq();
		byte[] GetFingerPrint();
		BigInteger ServerOnce();
		void Update();
		BigInteger CreateDh_ma();
		byte[] DhGetPrime();
		int Dh_mg();
		BigInteger NewDh_ma();
		//byte[] SHA1(byte[] value);
		//IRSAKey RsaKey { get; }
		byte[] DecryptClientMessage(byte[] encriptedData);
		byte[] EncryptWithServerNonce(byte[] serverNonce, byte[] newNonce, byte[] data);
		byte[] DecryptWithNonces(byte[] data, byte[] serverNonce, byte[] newNonce);
		Task SetAuthKey(BigInteger authKey, ulong authKeId);
	}
}
