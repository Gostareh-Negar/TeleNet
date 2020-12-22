using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.TL;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure
{
	public class TransportMessage : MTMessage
	{
		public bool Ack { get; set; }
	}
}
