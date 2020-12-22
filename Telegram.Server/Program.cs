using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MTProto.NET.Server;

namespace Telegram.Server
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().UseProtoServer().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
					webBuilder.UseKestrel(options =>
					{
						options.AllowSynchronousIO = true;
					});
					webBuilder.UseUrls("http://localhost:12345");
				});
	}
}
