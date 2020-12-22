using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IChatParticipant
	{
		int Id { get; set; }
		int ChatId { get; set; }
		int UserId { get; set; }
		int InviterId { get; set; }
		DateTime Date { get; set; }
	}
	public class ChatParticipantData : IChatParticipant
	{
		public int Id { get; set; }
		public int ChatId { get; set; }
		public int UserId { get; set; }
		public int InviterId { get; set; }
		public DateTime Date { get; set; }


	}
}
