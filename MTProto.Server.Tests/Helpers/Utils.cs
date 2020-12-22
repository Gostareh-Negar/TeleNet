using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server;
using System;
using System.Collections.Generic;
using System.Text;
using static MTProto.NET.Server.Infrastructure.Configurations.BusOptions;

namespace MTProto.Server.Tests.Helpers
{
	class Utils
	{
		public static RabbitMqSettings RabbitMq => new RabbitMqSettings
		{
			Server = "172.16.0.13",
			VirtualHost = "test",
			UserName = "test_user",
			Password = "test_user"
		};

		public static IWebHost CreateWebHost(
				Action<IMTServerBuilder> configure = null,
				Action<WebHostBuilderContext, IServiceCollection> configueServices = null,
				Action<IApplicationBuilder> configureAppBuilder = null,
				int port = 2365)
		{
			var result = WebHost.CreateDefaultBuilder()
				.ConfigureAppConfiguration(cfg => { })
				.ConfigureLogging(cfg =>
				{
					//cfg.AddConsole();

				})
				.ConfigureServices((c, s) =>
				{
					if (configueServices == null)
					{
						var builder = s.AddMTDefaultServer(c.Configuration);
						if (configure == null)
						{
							builder.AddBus(cfg =>
							{
								cfg.RabbitMq = RabbitMq;
							});
							builder.AddLiteCbStore();
							builder.AddDhLayer();
							builder.AddMTHttpTransport();

						}
						else
						{
							configure?.Invoke(builder);
						}
					}
					else
					{
						configueServices.Invoke(c, s);
					}
				})
				.UseEnvironment("Development")
				.UseUrls($"http://localhost:{port}")
				.Configure(app =>
				{
					if (configureAppBuilder == null)
					{
						app.UseProtoServer();
					}
					else
					{
						configureAppBuilder?.Invoke(app);
					}
				})
				.Build();
			result.Services.UseProtoServer();
			return result;

		}

	}
}
