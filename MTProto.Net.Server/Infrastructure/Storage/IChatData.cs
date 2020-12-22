using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IChatData
	{
		int Id { get; set; }
		string Title { get; set; }
	}
	public class ChatData : IChatData
	{
		public int Id { get ; set ; }
		public string Title { get; set; }
	}
}
