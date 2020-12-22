using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IMTRequestHandler
	{
		Task Handle(IMessageContext context);
	}
	public interface IMTRequestHandler<TReq> : IMTRequestHandler
	{

	}

}
