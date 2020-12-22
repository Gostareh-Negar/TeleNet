using MTProto.NET.Schema.MT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class SessionMessageQueue 
	{
		private IMTSession session;
		public static long MSGID;
		public int UserId { get; private set; }
		public ConcurrentDictionary<long, TransportMessage> messages = new ConcurrentDictionary<long, TransportMessage>();
		public SessionMessageQueue(IMTSession session)
		{
			//this.UserId = session.GetUserId();
			this.session = session;
		}
		public void AddMessage(TransportMessage message)
		{
			messages.AddOrUpdate(message.MsgId, message, (a, b) => message);
		}
		public IEnumerable<TransportMessage> GetPending(int max_count =100)
		{
			return messages.Values;
		}
		public void Clear()
		{
			messages = new ConcurrentDictionary<long, TransportMessage>();
		}
		public void Ack(long messageId)
		{
			if (messageId == MSGID)
			{

			}
			messages.TryRemove(messageId, out var _);
		}

	}
}
