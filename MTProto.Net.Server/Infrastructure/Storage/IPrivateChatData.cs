using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IPrivateChatData : IChatData
	{
		int UserId1 { get; set; }
		int UserId2 { get; set; }

	}
	public class PrivateChatData : IPrivateChatData
	{
		public int Id { get; set; }
		public int UserId1 { get; set; }
		public int UserId2 { get; set; }
		public string Title { get; set; }

	}
}
