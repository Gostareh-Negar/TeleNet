using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using MTProto.NET.Server;
using MTProto.Server.Tests.Helpers;

namespace MTProto.Server.Tests
{
	public class TestFixture
	{
		public class NullStartup
		{
			public static IConfiguration Configuration { get; protected set; }
			public NullStartup(IHostingEnvironment env)
			{
				//var builder = new ConfigurationBuilder();
				//.SetBasePath(env == null ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) : env.ContentRootPath)
				//.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				//.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				//.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libsettings.json"), optional: true)
				//.AddEnvironmentVariables();
				//Configuration = builder.Build();
			}
			public virtual void ConfigureServices(IServiceCollection services)
			{
			}
			public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
			{
			}
		}

		//public IHost CreateHost(Action<HostBuilderContext, IServiceCollection> configure)
		//{
		//	return Host.CreateDefaultBuilder()
		//		.ConfigureLogging(l =>
		//		{

		//		})
		//		.ConfigureServices((c, s) =>
		//		{
		//			configure?.Invoke(c, s);
		//		})
		//		.UseEnvironment("Development")
		//		.Build();

		//}
		public IWebHost CreateWebHost(
			Action<IMTServerBuilder> configure = null,
			Action<WebHostBuilderContext, IServiceCollection> configueServices = null, 
			Action<IApplicationBuilder> configureAppBuilder = null, 
			int port = 2365)
		{
			
			return Utils.CreateWebHost(configure, configueServices, configureAppBuilder, port);
			var result = WebHost.CreateDefaultBuilder()
				.ConfigureAppConfiguration(cfg => { })
				.ConfigureLogging(cfg =>
				{
					cfg.AddConsole();

				})
				.ConfigureServices((c, s) =>
				{
					if (configueServices == null)
					{
						var builder = s.AddMTDefaultServer(c.Configuration);
						if (configure == null)
						{
							builder.AddBus();
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
				.UseStartup<NullStartup>()
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
