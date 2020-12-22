using MTProto.NET.Server.Infrastructure.Encryption;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IMTCryptoService
	{
		byte[] DhGetPrime();
		int Dh_mg();
		BigInteger Dh_ma();
		byte [] SHA1(byte[] value);
		IRSAKey RsaKey { get; }
		byte[] DecryptClientMessage(byte[] encriptedData);
	}
}
