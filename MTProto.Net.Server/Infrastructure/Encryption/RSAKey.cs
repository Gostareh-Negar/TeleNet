using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Linq;


namespace MTProto.NET.Server.Infrastructure.Encryption
{
	/// <summary>
	/// https://stackoverflow.com/questions/9607295/calculate-rsa-key-fingerprint
	///  ssh-keygen -f tdlib.server.local
	///  ssh-keygen -f tdlib.server.local -e -m pem >tdlib.server.local.pem
	///  ssh-keygen -lf tdlib.server.local.pub
	///  ssh-keygen -E md5 -lf tdlib.server.local.pub >tdlib.server.local.pub.fingerprint.md5
	/// </summary>


	class RSAKey : IRSAKey
	{
		public string FingerPrint { get; set; }
		public string FingerPrint_MD5 { get; set; }
		public string PrivatePem { get; set; }
		public string PublicPem { get; set; }
		public string DhPrime { get; set; }

		public static RSAKey Default => new RSAKey
		{
			FingerPrint = DefaultKey.FingerPrint,
			FingerPrint_MD5 = DefaultKey.FingerPrint_MD5,
			PublicPem = DefaultKey.PUBLIC_PEM,
			PrivatePem = DefaultKey.PRIVATE_PEM,
			DhPrime = DefaultKey.c_hardcodedDhPrime,
		};

		public RSAKey()
		{

		}
		public RSAKey(string privatePem, string DhPrime)
		{

		}

		public byte[] GetFingerPrints()
		{
			var p = GetPublicKeyParams();
			var result = new List<Byte>();
			var modo = BitConverter.ToString(p.Modulus).Replace("-", "").ToLower();
			result.Add(254);
			result.Add(0);
			result.Add(1);
			result.Add(0);
			result.AddRange(p.Modulus);
			result.Add(3);
			result.AddRange(p.Exponent);
			var ret = SHA1.Create().ComputeHash(result.ToArray())
				.Reverse()
				.Take(8)
				.ToArray();
			return ret.Reverse().ToArray();
		}

		public Tuple<RSAParameters, RSAParameters> GetKeyParameters()
		{

			if (!PrivatePem.StartsWith("-----BEGIN RSA PRIVATE KEY-----"))
			{
				PrivatePem = "-----BEGIN RSA PRIVATE KEY-----" + "\r\n" +
					PrivatePem.Trim() + "\r\n" +
					"-----END RSA PRIVATE KEY-----";
			}
			PemReader pr = new PemReader(new StringReader(PrivatePem));
			var f = (AsymmetricCipherKeyPair)pr.ReadObject();
			RSAParameters _public = default;
			RSAParameters _private = default;
			if (f != null && f.Private as RsaPrivateCrtKeyParameters != null)
			{
				_private = DotNetUtilities.ToRSAParameters(f.Private as RsaPrivateCrtKeyParameters);
			}
			if (f != null && f.Public as RsaKeyParameters != null)
			{
				_public = DotNetUtilities.ToRSAParameters(f.Public as RsaKeyParameters);
			}
			return new Tuple<RSAParameters, RSAParameters>(_public, _private);
		}


		public RSAParameters GetPrivateKeyParams()
		{
			return GetKeyParameters().Item2;
			//PrivatePem = PrivatePem ?? DefaultKey.PRIVATE_PEM;
			//if (!PrivatePem.StartsWith("-----BEGIN RSA PRIVATE KEY-----"))
			//{
			//	PrivatePem = "-----BEGIN RSA PRIVATE KEY-----" + "\r\n" +
			//		PrivatePem.Trim() + "\r\n" +
			//		"-----END RSA PRIVATE KEY-----";
			//}
			//PemReader pr = new PemReader(new StringReader(PrivatePem));
			//var f = (AsymmetricCipherKeyPair)pr.ReadObject();
			//return DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)f.Private);
		}

		public RSAParameters GetPublicKeyParams()
		{
			return GetKeyParameters().Item1;
		}
	}
}
