using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MTProto.NET.Server;
using System.Linq;
using MTProto.NET.Server.Infrastructure;

namespace MTProto.Server.Tests.Specs
{
	[TestClass]
	public class SchemaService_Specs : TestFixture
	{
		[TestMethod]
		public async Task schema_should_return_constructor_value()
		{
			
			
			var target = MTServer.CreateDefaultServer(null, null).Build().Services.Schema();
			var items = target.GetAll().ToArray();

			await Task.CompletedTask;
		}
	}
}
