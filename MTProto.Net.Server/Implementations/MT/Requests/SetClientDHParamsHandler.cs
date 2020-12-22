using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Server.Infrastructure;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.MT.Requests
{
	public class SetClientDHParamsHandler : IProtoMessageHanddler<MTSetClientDhParams>
	{
		private readonly ILogger logger;
		private readonly IMTSessionManager manager;

		public SetClientDHParamsHandler(ILogger<SetClientDHParamsHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}

		public async Task<MTObject> Handle(IMessageContext<MTSetClientDhParams> context)
		{
			var result = await Task.FromResult<MTObject>(null);
			var req = context.Body;
			
			try
			{
				var session = this.manager.GetOrCreateAuthorizationSession(req.Nonce);
				var new_nonce = session.Data.GetNewNonce();
				var nonce = new_nonce.ToByteArrayUnsigned();
				var server_nonce = req.ServerNonce.ToByteArrayUnsigned();
				//var mem = TLSharp.Core.MTProto.Crypto.AES
				//	.DecryptWithNonces(req.EncryptedData, server_nonce, nonce);
				var mem = session.DecryptWithNonces(req.EncryptedData, server_nonce, nonce);
				mem = mem.Skip(20).ToArray();
				//var mem = RsaKey.DecryptClientMessage(req.EncryptedData);
				//var q = MTObjectSerializer.DeserializeEx(mem) as MTClientDhInnerData;
				var q = session.Services.Serialization().Deserialize(mem) as MTClientDhInnerData; ;// MTObjectSerializer.DeserializeEx(mem) as MTClientDhInnerData;
				if (q != null)
				{
					
					var gB = new BigInteger(1, q.GB);
					//var md_prime = new BigInteger(1, RsaKey.HardCodedPrime());
					var md_prime = new BigInteger(1, session.DhGetPrime());
					
					//var m_a = new BigInteger(1, context.Session.GetValue<byte[]>("m_a"));
					var m_a = session.Data.GetDh_ma();
					var newAuthKey = gB.ModPow(m_a, md_prime);
					var tmp = newAuthKey.ToByteArrayUnsigned();
					//var newAuthKeySha = SHA1.Create().ComputeHash(newAuthKey.ToByteArray());
					var newAuthKeySha = newAuthKey.ToByteArray().sha1();//

					var authKeyAux = newAuthKeySha.Take(8).ToArray();
					var authKeyId = newAuthKeySha.Reverse().Take(8).Reverse().ToArray();
					var authKeyId_ = BitConverter.ToUInt64(authKeyId, 0);

					//context.Session.AddOrUpdate<string>(newAuthKey.ToString(16), "Auth_Key");
					//context.Session.AddOrUpdate<byte[]>(authKeyId, "Auth_Key_Id");
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
					this.logger.LogDebug(
						$"NewOnceHash1='{ new BigInteger(1, newOnceHash1).ToString(16)}'\r\n " +
						$"AutKey='{newAuthKey.ToString(16)}'\r\n " +
						$"AutKeyId='{new BigInteger(1, authKeyId).ToString(16)}'\r\n " +
						$"AutKeyAux={new BigInteger(1, authKeyAux).ToString(16)}\r\n");
					//session.Data.AuthKeyId = authKeyId_;
					//session.Data.SetAuthKey(newAuthKey);
					//session.Update();
					await session.SetAuthKey(newAuthKey, authKeyId_);
				}
			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while tring to SetClientDhParams: \r\n{err.GetBaseException().Message}");
				throw;
			}
			return result;
		}
	}

		
	
	
}
