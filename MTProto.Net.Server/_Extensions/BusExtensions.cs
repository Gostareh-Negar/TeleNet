using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{

		public static Task<BusSubscription<T>> Register<T>(this IBus bus, bool durable = false, Action<BusSubscription<T>> configure = null, CancellationToken cancellationToken = default) where T:class
		{
			return bus.Subscribe<T>(s => {
				s.Durable = durable;
				s.IsRequestHandler = true;
				configure?.Invoke(s);
			});

		}
		public static Task<BusSubscription<IMessageContext<T>>> ProtoSubscribe<T>(this IBus bus, Action<BusSubscription<IMessageContext<T>>> configure) where T : MTObject
		{
			return bus.Subscribe<IMessageContext<T>>(configure);
		}
		public static async Task<IMessageContext> SendProto( this IBus bus, IMessageContext message, int timeOut =3000, SendModes mode= SendModes.Both,CancellationToken cancellationToken=default)
		{
			var result = await bus.Send(message, null, timeOut, mode, cancellationToken);
			if (result as IMessageContext != null)
			{
				return result as IMessageContext;
			}

			return MessageContext.Create(result);
		}
		public static Task<BusSubscription<T>> ProtoRegister<T>(this IBus bus, bool durable = false, Action<BusSubscription<T>> configure = null) where T : MTObject
		{

			return bus.Subscribe<T>(cfg =>
			{
				cfg.IsRequestHandler = true;
				cfg.Durable = durable;
				configure?.Invoke(cfg);
			});
		}
	}
}
