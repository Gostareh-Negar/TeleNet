
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Server.Infrastructure;


namespace MTProto.NET.Server.Implementations.MT.Requests
{
	public class ReqPqHandler : IProtoMessageHanddler<MTReqPq>
	{
		private readonly ILogger<ReqPqHandler> logger;
		private readonly IMTAuthorizationService authorizationService;
		private readonly IMTSessionManager manager;

		public ReqPqHandler(ILogger<ReqPqHandler> logger,  IMTSessionManager manager)
		{
			this.logger = logger;
			this.manager = manager;
		}
		public async Task<MTObject> Handle(IMessageContext<MTReqPq> context)
		{
			MTResPQ result = await Task.FromResult<MTResPQ>(new MTResPQ());
			try
			{
				var req = context.Body;
				var session = this.manager.GetOrCreateAuthorizationSession(req.Nonce.ToString(16));
				var vector = new MTProto.NET.Schema.TL.TLVector<long>();
				var fingerPrint = BitConverter.ToInt64(session.GetFingerPrint(), 0); ;
				vector.Add(fingerPrint);
				result.Nonce = req.Nonce;
				result.Pq = BitConverter.GetBytes(session.m_pq());
				result.ServerPublicKeyFingerprints = vector;
				result.ServerNonce = session.ServerOnce();
				session.Update();
				this.logger.LogTrace(
					$"MTReqPq successfully handled. Result: {result} ");
			}
			catch (Exception err)
			{
				this.logger.LogDebug(
					$"An error occured while trying to handel this request. {err}");
				throw;
			}
			return result;
		}
	}
}
