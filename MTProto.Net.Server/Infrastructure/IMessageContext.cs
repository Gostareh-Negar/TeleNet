using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IMessageContext
	{
		IDictionary<string, string> Headers { get; }
		IDictionary<string, object> Cache { get; }
		object Body { get; }
		string Topic { get; }

		IMTServiceProvider Services { get; }


		IMessageContext<T> Cast<T>() where T : class;

		IMessageContext Cast(Type type = null);


	}
	public interface IMessageContext<out T> : IMessageContext 
	{
		new T Body { get; }
	}
}
