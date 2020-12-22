using MTProto.NET;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Serializers;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TDLib.Server2;

namespace TelegramServer.Handlers
{
	public class SetClientDHParamsHandler : IMTProtoHandler
	{
		public async Task<MTObject> Handle(IMTProtoContext context)
		{
			var result = await Task.FromResult<MTObject>(null);
			var new_once_str = context.Session.GetValue<string>("NewNonce");
			var new_nonce = new BigInteger(new_once_str, 10);
			var req = context.Request as MTSetClientDhParams;
			try
			{
				var nonce = new_nonce.ToByteArrayUnsigned();
				var server_nonce = req.ServerNonce.ToByteArrayUnsigned();
				var mem = TLSharp.Core.MTProto.Crypto.AES
					.DecryptWithNonces(req.EncryptedData, server_nonce, nonce);
				mem = mem.Skip(20).ToArray();

				//var mem = RsaKey.DecryptClientMessage(req.EncryptedData);
				var q = MTObjectSerializer.DeserializeEx(mem) as MTClientDhInnerData;
				if (q != null)
				{
					var gB = new BigInteger(1, q.GB);
					var md_prime = new BigInteger(1, RsaKey.HardCodedPrime());
					var m_a = new BigInteger(1, context.Session.GetValue<byte[]>("m_a"));
					var newAuthKey = gB.ModPow(m_a, md_prime);
					
						

					var tmp = newAuthKey.ToByteArrayUnsigned();
					var newAuthKeySha = SHA1.Create().ComputeHash(newAuthKey.ToByteArray());


					var authKeyAux = newAuthKeySha.Take(8).ToArray();
					var authKeyId = newAuthKeySha.Reverse().Take(8).Reverse().ToArray();
					context.Session.AddOrUpdate<string>(newAuthKey.ToString(16), "Auth_Key");
					context.Session.AddOrUpdate<byte[]>(authKeyId, "Auth_Key_Id");
					var newOnceHash1 = SHA1.Create().ComputeHash(
						new_nonce.ToByteArrayUnsigned()
							.Concat(new byte[] { (byte)1 })
							.Concat(authKeyAux)
							.ToArray()).Reverse().Take(16).Reverse().ToArray();
					



					var expectedHashData = nonce.Concat(newAuthKeySha.Take(8)).ToList();
					expectedHashData.Insert(32, (byte)1);
					var newNonceHashLower128Array = SHA1.Create().ComputeHash(expectedHashData.ToArray())
						.Skip(4).ToArray();
					var number = new BigInteger(1, newNonceHashLower128Array.Take(16).ToArray());

					result = new MTDhGenOk
					{
						NewNonceHash1 = new BigInteger(1, newOnceHash1),
						Nonce = req.Nonce,
						ServerNonce = req.ServerNonce,
					};




				}
			}
			catch (Exception err)
			{

			}


			return result;

		}
	}
}
