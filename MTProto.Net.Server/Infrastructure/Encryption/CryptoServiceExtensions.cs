using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using MTProto.NET.Server.Infrastructure.Encryption;
using System.Linq;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Infrastructure.Encryption.Helpers.Legacy;
using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server.Infrastructure.Configurations;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		internal static IServiceCollection AddCryptoServices(this IServiceCollection services)
		{
			services.AddTransient<IMTCryptoService, MTCryptoService>();
			services.AddTransient<IRSAKey>(p => ServerOptions.Instance.ServerKey);

			return services;
		}
		public static IMTCryptoService EncryptionServices(this IMTServiceProvider provider)
		{
			return provider.GetService<IMTCryptoService>();
		}
		public static RSAParameters GetPrivateKeyParams(string PrivatePem)
		{
			return GetKeyParameters(PrivatePem).Item2;
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
		public static Tuple<RSAParameters, RSAParameters> GetKeyParameters(string PrivatePem)
		{
			PrivatePem = PrivatePem ?? DefaultKey.PRIVATE_PEM;
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
		public static RSAParameters GetPublicKeyParams(string pem)
		{
			return GetKeyParameters(pem).Item1;
		}
		public static RSAParameters GetPublicKeyParamsEx(string publicPem)
		{
			publicPem = publicPem ?? DefaultKey.PUBLIC_PEM;
			if (!publicPem.StartsWith("-----BEGIN RSA PUBLIC KEY-----"))
			{
				publicPem = "-----BEGIN RSA PUBLIC KEY-----" + "\r\n" +
					publicPem.Trim() + "\r\n" +
					"-----END RSA PUBLIC KEY-----";
			}
			PemReader pr = new PemReader(new StringReader(publicPem));
			var f = pr.ReadObject() as RsaKeyParameters;
			return DotNetUtilities.ToRSAParameters(f);

		}

		public static byte[] GetFingerPrint(string pem)
		{
			var p = GetPublicKeyParams(pem);
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
		public static byte[] GetFingerPrint(IRSAKey key)
		{
			var p = GetPublicKeyParams(key.PrivatePem);
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

		public static AESKeyData GenerateKeyDataFromNonces(this IMTCryptoService utility, byte[] serverNonce, byte[] newNonce)
		{
			return AES.GenerateKeyDataFromNonces(serverNonce, newNonce);
		}
		public static byte[] EncryptWithServerNonce(this IMTCryptoService utility, byte[] serverNonce, byte[] newNonce, byte[] data)
		{
			var key = AES.GenerateKeyDataFromNonces(serverNonce, newNonce);
			return AES.EncryptAES(key, data);
		}
		public static byte[] sha1(this byte[] source)
		{
			using (var sha = SHA1.Create())
			{
				return sha.ComputeHash(source);
			}
		}
	}
}


