using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Contracts.Messages
{
	public class NewMessageCreated
	{
		public int FromUserId { get; set; }
		public int ToPeerId { get; set; }
		public int PeerIdType { get; set; }
		public string Text { get; set; }

		public int Date { get; set; }
		public int Id { get; set; }
		
	}
}
