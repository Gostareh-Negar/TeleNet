using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTProto.NET.Server;
using MTProto.NET.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Server.Tests.Specs
{
	[TestClass]
	public class MessageContext_Specs
	{
		public class Message
		{
			public string Value { get; set; }
		}
		[TestMethod]
		public async Task should_be_able_to_serialize_context()
		{
			
			var context = MessageContext.Create(new Message { Value = "test" }).Cast(null) as IMessageContext<Message>;
			var packet = context.Serialize();

			var message = MTServer.Instance.Services.DeserializeMessage(packet) as IMessageContext<Message>;
			Assert.IsNotNull(context);

		}
	}
}
