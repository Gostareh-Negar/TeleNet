using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Encryption
{
	public interface IRSAKey
	{
		string PrivatePem { get; }
		string PublicPem { get; }
		string DhPrime { get; }
		byte[] GetFingerPrints();
		RSAParameters GetPrivateKeyParams();
		RSAParameters GetPublicKeyParams();

	}
}
