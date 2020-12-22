using MTProto.NET;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Serializers;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TDLib.Server2;


namespace TelegramServer.Handlers
{
	
	public class ReqDHParamsHandler : IMTProtoHandler
	{
		IMTProtoContext context;

		private MTObject Accept(MTPQInnerData req)
		{
			MTObject result = null;
			var m_dhPrime = RsaKey.HardCodedPrime();
			var m_g = 3;
			var m_a = new byte[256];
			new Random().NextBytes(m_a);
			context.Session.AddOrUpdate<byte[]>(m_a, "m_a");
			var vvv = new BigInteger("3");
			var m_gA = vvv.ModPow(new BigInteger(1, m_a), new BigInteger(1, m_dhPrime));


			var data = new MTServerDhInnerData();
			data.Nonce = req.Nonce;
			data.ServerNonce = req.ServerNonce;
			data.G = m_g;
			data.DhPrime = m_dhPrime;
			data.GA = m_gA.ToByteArray();

			var inner_data = MTObjectSerializer.SerializeEx(data);
			var sha = SHA1.Create().ComputeHash(inner_data);
			var length = sha.Length + inner_data.Length;
			byte[] padding = new byte[] { };
			if (length % 16 != 0)
			{
				padding = new byte[16 - (length % 16)];
			}
			try
			{
				context.Session.AddOrUpdate<string>(req.NewNonce.ToString(10), "NewNonce");
				var gggg = req.ServerNonce.ToByteArray();
				var _gggg = req.NewNonce.ToByteArray();
				var all_data = sha.Concat(inner_data).Concat(padding).ToArray();
				var aesData = TLSharp.Core.MTProto.Crypto.AES.GenerateKeyDataFromNonces(gggg, _gggg);
				var encryptedAnswer = TLSharp.Core.MTProto.Crypto.AES.EncryptAES(aesData, all_data);
				//var encryptedAnswer = RsaKey.AesEncriptWiteNonce(_gggg,gggg,
				//	sha.Concat(inner_data).Concat(padding).ToArray());
				result = new MTServerDhParamsOk
				{
					EncryptedAnswer = encryptedAnswer,
					Nonce = req.Nonce,
					ServerNonce = req.ServerNonce
				};
			}
			catch (Exception err)
			{
			}




			//result = true;

			return result;



		}
		public async Task<MTObject> Handle(MTObject request)
		{
			await Task.CompletedTask;
			MTObject result = null;
			var req = request as MTReqDhParams;
			var fff = RsaKey.m_pq();
			var ser = RsaKey.ServerOnce();
			//var ff = RsaKey.DEC(req.EncryptedData);
			var par = RsaKey.GetPrivateKeyParams();
			var par1 = RsaKey.GetPublicKeyParams();
			//https://csharp.hotexamples.com/examples/TLSharp.Core.MTProto.Crypto/BigInteger/-/php-biginteger-class-examples.html
			var big1 = new BigInteger(req.EncryptedData);
			try
			{


				var big3 = big1.ModPow(new BigInteger(1, par.D), new BigInteger(1, par1.Modulus)).ToByteArrayUnsigned();
				var big4 = big1.ModPow(new BigInteger(1, par.D), new BigInteger(1, par.Modulus)).ToByteArrayUnsigned();
				var sha1 = big3.Take(20).ToArray();
				var rem = big3.Skip(20).ToArray();
				rem = RsaKey.DecryptClientMessage(req.EncryptedData);
				var memory = new MemoryStream(rem, false);
				var reader = new BinaryReader(memory);
				var innerData = MTObjectSerializer.Deserialize(reader) as MTPQInnerData;
				if (innerData != null)
				{
					this.context.Session.AddOrUpdate(innerData);
					this.context.Session.GetValue<MTPQInnerData>();
					result = this.Accept(innerData);
				}
			}
			catch (Exception err)
			{

			}


			//var big = new BigInteger(req.EncryptedData).ModPow(new BigInteger(par.Exponent), new BigInteger(par.Modulus)).ToByteArray();




			return result;

		}

		public Task<MTObject> Handle(IMTProtoContext context)
		{
			this.context = context;
			return Handle(context.Request);
		}
	}
}
