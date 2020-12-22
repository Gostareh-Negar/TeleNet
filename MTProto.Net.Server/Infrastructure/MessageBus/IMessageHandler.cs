using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IMessageHandler { }
	public interface IMessageHandler<T,TResp>:IMessageHandler 
	{
		Task<TResp> Handle(IMessageContext<T> context);
	}
	public interface IMessageHandler<T> : IMessageHandler<T, object> 
	{ 
	}
	public interface IGenericMessageHandler : IMessageHandler<object> { }



}
