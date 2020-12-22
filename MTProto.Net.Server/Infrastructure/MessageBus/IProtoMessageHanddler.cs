using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{

	
	public interface IProtoMessageHanddler:IMessageHandler
	{
		//Task<MTObject> Handle(MTObject message);
	}
	public interface IProtoMessageHanddler<T> : IProtoMessageHanddler,IMessageHandler<T,MTObject> where T : MTObject
	{
		//Task<MTObject> Handle(IMessageContext<T> context);
	}

}
