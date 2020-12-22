using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server.Infrastructure.Configurations;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Infrastructure.Transports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MTProto.NET.Server.Infrastructure.Storage;

namespace MTProto.NET.Server
{
	public interface IMTService
	{

	}
	public interface IMTServer ///: IServiceProvider
	{
		IMTServiceProvider Services { get; }
	}
	public interface IMTServiceProvider : IServiceProvider
	{
		T GetService<T>();
	}

	public interface IMTServerBuilder
	{
		IServiceCollection ServiceCollection { get; }
		IConfiguration Configuration { get; }
		IMTServerBuilder Configure(Action<ServerOptions> configure);
		IMTServer Build();
	}
	public class MTServer : IMTServer, IMTServerBuilder, IMTServiceProvider
	{
		private IServiceCollection _serviceCollection;
		private IServiceProvider _serviceProvider;
		private IConfiguration _configuration;
		private Action<ServerOptions> _configure;

		public MTServer()
		{
		}

		internal static MTServer Internal = new MTServer();
		internal static IMTServiceProvider Services => Internal;
		public static IMTServer Instance => Internal;

		public static IBus Bus => Services.GetService<IBus>();

		public IServiceCollection ServiceCollection
		{
			get
			{
				this._serviceCollection = this._serviceCollection ?? new ServiceCollection();
				return this._serviceCollection;
			}
		}
		public IServiceProvider ServiceProvider
		{

			get
			{
				this._serviceProvider = this._serviceProvider ?? this.ServiceCollection.BuildServiceProvider();
				return this._serviceProvider;
			}
		}

		public IConfiguration Configuration
		{
			get
			{
				if (this._configuration == null)
				{
					var builder = new ConfigurationBuilder();
					this._configuration = builder.Build();
				}
				return this._configuration;
			}
		}

		public IMTServerBuilder Configure(Action<ServerOptions> configure)
		{
			var options = ServerOptions.Instance;
			this._configure?.Invoke(options);
			configure?.Invoke(options);
			this.ServiceCollection.AddSingleton(options);
			return this;
		}
		public static IMTServerBuilder CreateDefaultServer(IServiceCollection services, IConfiguration configuration, Action<ServerOptions> configure = null)
		{
			Internal = Internal ?? new MTServer();
			Internal._serviceCollection = services;
			Internal._configuration = configuration;
			Internal._configure = configure;

			var result = Internal;
			result.ServiceCollection.AddMTServer(result.Configuration, configure);


			return Internal;
		}


		public object GetService(Type serviceType)
		{
			return this.ServiceProvider?.GetService(serviceType);
		}


		public void Use(IServiceProvider provider)
		{
			this._serviceProvider = provider;
		}
		public void Use(IApplicationBuilder builder)
		{
			Use(builder.ApplicationServices);
			builder.UseMiddleware<MTHttpTransport>();
		}

		public IMTServer Build()
		{
			return this;
		}

		public T GetService<T>()
		{
			var result = this.ServiceProvider.GetService<T>();
			if (result == null && !typeof(T).IsAbstract)
			{
				try { result = ActivatorUtilities.CreateInstance<T>(this.ServiceProvider); }
				catch { }
			}
			return result;
		}


		IMTServiceProvider IMTServer.Services => this;

		
	}
}
