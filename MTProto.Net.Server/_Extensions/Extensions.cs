using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server.Infrastructure.Configurations;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Infrastructure.Implementations;

using MTProto.NET.Server.Infrastructure.Transports;
using System;
using System.Linq;
using MTProto.NET.Server.Schema;
using MTProto.NET.Server.Implementations.MT.Requests;
using Microsoft.Extensions.Hosting;
using MTProto.NET.Server.Infrastructure.Storage;
using MTProto.NET.Server.Infrastructure.Storage.Implementations;
using MTProto.NET.Server.Services.Account;
using MTProto.NET.Server.Services.Contacts;
using MTProto.NET.Server.Services.Messages;
using MTProto.NET.Server.Services.Upload;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		internal static IServiceProvider ServiceProvider => MTServer.Internal;
		public static IMTServerBuilder AddMTDefaultServer(this IServiceCollection serviceCollection, IConfiguration configuration, Action<ServerOptions> configure = null)
		{
			return MTServer.CreateDefaultServer(serviceCollection, configuration, configure);
		}

		internal static IServiceCollection AddMTServer(this IServiceCollection services, IConfiguration configuration, Action<ServerOptions> configure = null)
		{
			var options = ServerOptions.Instance;
			configure?.Invoke(options);
			services.AddTransient<IMTServiceProvider>(p => MTServer.Internal);
			services.AddTransient<IMTServer>(p => MTServer.Internal);
			services.AddSingleton<MTUtilityServices>();
			services.AddTransient<IMTUtilityService>(p => p.GetService<MTUtilityServices>());
			services.AddSingleton(MTMapper.Instance);
			services.AddSingleton<IMTMapper,MTMapper>(p =>MTMapper.Instance);
			services.AddSingleton<MTObjectFactory>();
			services.AddSingleton<IMTObjectFactory>(p => p.GetService<MTObjectFactory>());
			services.AddTransient<IStore, Store>();
			services.AddSingleton(options);
			//services.AddMTAuthorization(configuration, null);
			services.AddLiteDbStorage(configuration, null);

			services.AddSingleton<UpdateManager>();
			services.AddTransient<IHostedService>(p => p.GetService<UpdateManager>());
			services.AddTransient<IUpdateManager>(p=>p.GetService<UpdateManager>());
			services.AddSingleton<MTSessionManager>();
			services.AddSingleton<IMTSessionManager>(p =>p.GetService<MTSessionManager>());
			services.AddSingleton<IHostedService>(p => p.GetService<MTSessionManager>());
			services.AddTransient<IMessageHandler>(p => p.GetService<MTSessionManager>());
			services.AddTransient<MTSession>();
			services.AddTransient<IUserManager, UserManager>();
			services.AddTransient<IChatManager, ChatManager>();

			services.AddSerialization();
			services.AddCryptoServices();


			// Services
			// Account
			services.AddTransient<IAccountProfileService, AccountProfileService>();
			// Contact
			services.AddTransient<IContactService, ContactService>();
			// Messaging
			services.AddTransient<IMessagingService, MessagingService>();
			// Upload
			services.AddTransient<IUploadService, UploadService>();


			return services;
		}
		internal static T GetService<T>() where T : class
		{
			var result = ServiceProvider?.GetService<T>();
			if (result == null && !typeof(T).IsAbstract)
			{
				try
				{
					result = ActivatorUtilities.CreateInstance<T>(ServiceProvider);
				}
				catch { }
			}
			return result;
		}
		public static IBus Bus(this IMTServiceProvider provider)
		{
			return provider.GetService<IBus>();
		}
		public static ISchemaService Schema(this IMTServiceProvider provider)
		{
			return provider.GetService<ISchemaService>() ?? new SchemaService();
		}


		public static IMTServerBuilder AddLiteCbStore(this IMTServerBuilder builder, Action<StorageOptions> configure = null)
		{
			builder.ServiceCollection.AddLiteDbStorage(builder.Configuration, configure);
			return builder;
		}
		public static IMTServerBuilder AddDhLayer(this IMTServerBuilder builder, Action<BusOptions> configure = null)
		{

			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, ReqPqHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, ReqDHParamsHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, SetClientDHParamsHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, GetNearestDcHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, PingHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, TLSignInHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, LogOutHandler>();
			return builder;
		}

		public static IMTServerBuilder AddBus(this IMTServerBuilder builder, Action<BusOptions> configure = null)
		{
			builder.ServiceCollection.AddMTBus(builder.Configuration, configure);
			return builder;
		}
		
		internal static IServiceCollection AddMTBus(this IServiceCollection serviceCollection, IConfiguration configuration, Action<BusOptions> configure = null)
		{
			var options = BusOptions.Instance;
			configuration.GetSection(typeof(HttpTransportOptions).Name).Bind(options);
			configure?.Invoke(options);
			serviceCollection.AddSingleton(options);
			serviceCollection.AddSingleton<Bus>();
			serviceCollection.AddSingleton<IBus>(p => p.GetService<Bus>());
			serviceCollection.AddSingleton<IHostedService>(p => p.GetService<Bus>());
			serviceCollection.AddSingleton<RabbitMqHelper>();
			return serviceCollection;
		}

		public static IMTServerBuilder AddMTHttpTransport(this IMTServerBuilder builder, Action<HttpTransportOptions> configure = null)
		{

			builder.ServiceCollection.AddMTHttpTransport(builder.Configuration, configure);
			return builder;
		}
		internal static IServiceCollection AddMTHttpTransport(this IServiceCollection serviceCollection, IConfiguration configuration, Action<HttpTransportOptions> configure = null)
		{
			var options = HttpTransportOptions.Instance;
			configuration.GetSection(typeof(HttpTransportOptions).Name).Bind(options);
			configure?.Invoke(options);
			serviceCollection.AddSingleton(options);
			serviceCollection.AddTransient<IMTTransport, MTHttpTransport>();
			serviceCollection.AddTransient<MTHttpTransport>();
			serviceCollection.AddHttpContextAccessor();
			return serviceCollection;

		}
		public static void UseProtoServer(this IServiceProvider provider)
		{
			MTServer.Internal.Use(provider);
		}
		public static void UseProtoServer(this IApplicationBuilder builder)
		{
			MTServer.Internal.Use(builder);
		}
		public static IHost UseProtoServer(this IHost provider)
		{
			MTServer.Internal.Use(provider.Services);
			return provider;
		}



	}
}
