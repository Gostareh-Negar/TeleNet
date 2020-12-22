using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Infrastructure.Helpers;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.MT.Requests
{

	class ReqDHParamsHandler : IProtoMessageHanddler<MTReqDhParams>
	{
		private readonly ILogger<ReqDHParamsHandler> logger;
		private readonly IMTSessionManager manager;

		public ReqDHParamsHandler(ILogger<ReqDHParamsHandler> logger, IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}
		private MTObject Accept(IAuthorizationSession session, MTPQInnerData req)
		{
			MTObject result = null;
			var m_dhPrime = session.DhGetPrime();
			var m_a = session.NewDh_ma();
			var m_g = session.Dh_mg();
			var m_gA = new BigInteger(m_g.ToString()).ModPow(m_a, new BigInteger(1, m_dhPrime));

			session.Data.SetNewNoce(req.NewNonce);
			session.Data.SetDh_ma(m_a);
			session.Data.Set_mgA(m_gA);

			var data = new MTServerDhInnerData();
			data.ServerTime = (int)(DateTimeHelper.CurrentMsecsFromEpoch() / 1000ul);
			data.Nonce = req.Nonce;
			data.ServerNonce = req.ServerNonce;
			data.G = m_g;
			data.DhPrime = m_dhPrime;
			data.GA = m_gA.ToByteArray();
			var inner_data = data.ToByteArray();
			var sha = inner_data.sha1();
			var length = sha.Length + inner_data.Length;
			byte[] padding = new byte[] { };
			if (length % 16 != 0)
			{
				padding = new byte[16 - (length % 16)];
			}
			try
			{
				var message = sha.Concat(inner_data).Concat(padding).ToArray();
				var encryptedAnswer = session.EncryptWithServerNonce(req.ServerNonce.ToByteArray()
					, req.NewNonce.ToByteArray()
					, message);
				result = new MTServerDhParamsOk
				{
					EncryptedAnswer = encryptedAnswer,
					Nonce = req.Nonce,
					ServerNonce = req.ServerNonce
				};
			}
			catch (Exception err)
			{
				this.logger.LogDebug(
					$"An error occured while trying to accept 'ReqDHParams': {err.Message}");
			}
			return result;
		}


		public async Task<MTObject> Handle(IMessageContext<MTReqDhParams> context)
		{
			var result = await Task.FromResult<MTObject>(null);
			try
			{
				var request = context.Body;
				var session = this.manager.GetOrCreateAuthorizationSession(request.Nonce.ToString(16));
				var par = session.Services.EncryptionServices().RsaKey.GetPrivateKeyParams();
				////https://csharp.hotexamples.com/examples/TLSharp.Core.MTProto.Crypto/BigInteger/-/php-biginteger-class-examples.html
				var rem = new BigInteger(1, request.EncryptedData)
					.ModPow(new BigInteger(1, par.D), new BigInteger(1, par.Modulus))
					.ToByteArrayUnsigned()
					.Skip(20)
					.ToArray();

				var innerData = session.Services.Serialization().Deserialize(rem) as MTPQInnerData;
				if (innerData != null)
				{
					result = this.Accept(session, innerData);
				}
				else
				{

				}
				if (result == null)
				{
					result = new MTServerDhParamsFail
					{
						Nonce = request.Nonce.ToByteArray(),
						ServerNonce = request.ServerNonce.ToByteArray(),
						NewNonceHash = request.Nonce.ToByteArray()
					};
				}
				session.Update();

			}
			catch (Exception err)
			{
				this.logger.LogDebug(
					$"An errro occured while trying to handel {typeof(MTReqDhParams).Name}:\r\n {err} ");

			}
			return result;
		}
	}
}
