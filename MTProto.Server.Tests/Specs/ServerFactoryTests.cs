using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTProto.NET.Server;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net;
using System.IO;
using System;
using MTProto.NET.Server.Infrastructure.Serialization;
using MTProto.NET.Schema.MT.Requests;
using System.Collections.Generic;
using System.Linq;

namespace MTProto.Server.Tests
{

	[TestClass]
	public class ServerFactoryTests : TestFixture
	{

		public Tin Gen<Tin>(Tin value)
		{
			return value;
		}
		public void DoThis<T>(Action<object> action, Type[] types, params object[] args)
		{
			var method = action.Method;
			var g0 = method.IsGenericMethod;
			//var g1 = method.IsConstructedGenericMethod;
			var g2 = method.GetGenericMethodDefinition()
				.MakeGenericMethod(types)
				.Invoke(action.Target, args);


			var _type = method.DeclaringType;
			//.MakeGenericMethod(type)
			//.Invoke(action.Target, new object[]{ });

		}
		[TestMethod]
		public async Task how_to_build_a_server()
		{

			//Extensions.MakeGenericFunctionCall<int>(this.Gen<object>, typeof(int), 1);



			//DoThis<int>(this.Gen<object>, new Type[] { typeof(int), typeof(int) }, 12);
			//var f = this.Gen;
			var host = this.CreateWebHost(b =>
			{
				b
				.AddMTHttpTransport()
				.AddBus()
				.AddDhLayer();


			}, configureAppBuilder: app =>
			{
				//app.Use(async (c, n) => {

				//	await Task.Delay(100);

				//});
				app.UseProtoServer();
				//app.UseMvc();
			});

			await host.StartAsync();
			var client = new HttpClient();
			//await client.GetAsync("http://localhost:2365/lll");
			//WebRequest request = WebRequest.Create("http://localhost:2365/lll");
			using (var binary = new MTStreamWriter())
			{
				binary.Write(Convert.ToUInt64(0));
				binary.Write(Convert.ToUInt64(1));
				binary.Write(Convert.ToInt32(1));
				binary.Write(new MTReqPq
				{
					Nonce = new Org.BouncyCastle.Math.BigInteger("123", 10),
				});
				binary.Flush();
				var stream = binary.BaseStream;
				stream.Seek(0, SeekOrigin.Begin);
				await client.PostAsync("http://localhost:2365/lll", new StreamContent(stream));
			}


			await Task.Delay(1 * 60 * 1000);
			await host.StopAsync();





		}
	}
}
