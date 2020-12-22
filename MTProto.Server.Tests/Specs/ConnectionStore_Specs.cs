using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server.Infrastructure.Helpers;
using MTProto.NET.Schema.TL.Auth;
using MTProto.NET.Serializers;
using MTProto.NET.Server;

namespace MTProto.Server.Tests.Specs
{
	[TestClass]
	public class ConnectionStore_Specs : TestFixture
	{
		[TestMethod]
		public async Task connection_store_works()
		{
			var host = this.CreateWebHost();

			var sent_code = new TLSentCode()
			{
				Flags = 0,
				PhoneCodeHash = "hiushi",
				Type = new TLSentCodeTypeSms
				{
					Length = 4,
				}
			};
			sent_code.ToByteArray();
			//MTObjectSerializer.Serialize(sent_code);
			var f = DateTimeHelper.CurrentMsecsFromEpoch();
			//var host = this.CreateWebHost();
			using (var store = host.Services.GetService<IConnectionStore>())
			{
				var data = store.Upsert(new ConnectionData { ConnectionId = "babak" });
				var data_read = store.GetConnectionId("babak");


			}
			var d = new SessionData
			{
				ClientNonce = "nonce"
			}.Upsert();
		}

	}
}
