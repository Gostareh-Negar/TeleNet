using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Infrastructure.Configurations;
using Org.BouncyCastle.Math;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace MTProto.NET.Server.Infrastructure.Encryption
{
	class MTCryptoService : IMTCryptoService
	{
		private readonly ILogger<MTCryptoService> logger;
		private readonly ServerOptions serverOptions;
		private readonly IRSAKey ServerKey;

		public IRSAKey RsaKey => ServerKey;

		public MTCryptoService(ILogger<MTCryptoService> logger, ServerOptions serverOptions)
		{
			this.logger = logger;
			this.serverOptions = serverOptions;
			this.ServerKey = this.serverOptions.ServerKey;
		}
		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}
		public byte[] DhGetPrime()
		{
			return StringToByteArray(serverOptions.ServerKey.DhPrime);

		}

		public int Dh_mg()
		{
			return 3;
		}
		public BigInteger Dh_ma()
		{
			var m_a = new byte[256];
			new Random().NextBytes(m_a);
			var result = new BigInteger(1, m_a);
			return result;
		}

		public byte[] SHA1(byte[] value)
		{
			using (var s = System.Security.Cryptography.SHA1.Create())
			{
				return s.ComputeHash(value);
			}
		}

		public  byte[] DecryptClientMessage(byte[] encriptedData)
		{
			var big1 = new BigInteger(encriptedData);
			var par = this.RsaKey.GetPrivateKeyParams();
			var big4 = big1.ModPow(new BigInteger(1, par.D), new BigInteger(1, par.Modulus)).ToByteArrayUnsigned();
			return big4.Skip(20).ToArray();

		}

	}
}
