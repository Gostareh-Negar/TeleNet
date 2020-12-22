using MTProto.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramServer.Handlers
{
	public interface IMTProtoHandler
	{
		Task<MTObject> Handle(IMTProtoContext context);
	}
	public interface IMTProtoHandler<in TRequest> : IMTProtoHandler where TRequest : MTObject
	{
		//Task Handle(IMTProtoContext request);
	}
	public abstract class MTProtoHandler<TReq, TResponse> : IMTProtoHandler<TReq> where TReq : MTObject where TResponse : MTObject
	{
		public abstract Task<TResponse> Handle(TReq request);

		protected IMTProtoContext Context { get; private set; }

		public virtual async Task<MTObject> Handle(IMTProtoContext context)
		{
			this.Context = context;
			return await Handle(context.Request as TReq);

		}
	}
}
