using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Server.Tests.Helpers
{
	public class TestMessage
	{
		public string Message { get; set; }
	}

	
	public class TestMessageReply
	{
		public string Message { get; set; }
	}

	public class PublishTestMessageCommand
	{
		public string Message { get; set; }
	}
}
