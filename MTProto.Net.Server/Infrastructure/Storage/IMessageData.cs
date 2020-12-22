using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IMessageData
	{
		int Id { get; set; }

		int? FromId { get; set; }
		int ToPeerId { get; set; }
		/// <summary>
		/// 0 for user, 1 for chat
		/// </summary>
		int ToPeerIdType { get; set; }
		string Message { get; set; }
		
		int Date { get; set; }
	}
	public class MessageData : IMessageData
	{
		public int Id { get; set; }

		public int? FromId { get; set; }
		public int ToPeerId { get; set; }
		/// <summary>
		/// 0 for user, 1 for chat
		/// </summary>
		public int ToPeerIdType { get; set; }
		public string Message { get; set; }
		public int Date { get; set; }
	}
}
