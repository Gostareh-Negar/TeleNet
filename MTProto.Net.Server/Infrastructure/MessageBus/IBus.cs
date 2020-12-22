using MTProto.NET.Server.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public delegate Task<object> MessageHandler(IMessageContext message);
	public delegate Task<object> MessageHandler<in T>(IMessageContext<T> message) where T : class;
	public delegate Task NotificationHandler<in T>(IMessageContext<T> message) where T : class;

	public enum SendModes
	{
		InternalOnly,
		ExternalOnly,
		Both
	}
	public interface IBus
	{
		Task<BusSubscription> Subscribe(Action<BusSubscription> configure, CancellationToken cancellationToken = default);
		Task<BusSubscription<T>> Subscribe<T>(Action<BusSubscription<T>> configure, string topic = null, CancellationToken cancellationToken = default) where T : class;
		Task Publish(object message, string topic = null, CancellationToken cancellationToken = default);
		Task<object> Send(object message, string topic = null, int timout = 30000, SendModes mode = SendModes.Both, CancellationToken cancellationToken = default);

	}
}
