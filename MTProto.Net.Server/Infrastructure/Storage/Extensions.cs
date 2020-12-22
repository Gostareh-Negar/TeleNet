using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server.Infrastructure.Storage;
using MTProto.NET.Server.Infrastructure.Storage.Implementations;
using MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal;
using MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		internal static IServiceCollection AddLiteDbStorage(this IServiceCollection services, IConfiguration configuration, Action<StorageOptions> configure)
		{
			var options = StorageOptions.Instance;
			configure?.Invoke(options);
			if (!services.Any(x => x.ServiceType == options.GetType()))
			{
				services.AddTransient<IConnectionStore, LiteDbConnectionStore>();
				services.AddTransient<ISessionStore, LiteDbSessionStore>();
				services.AddTransient<IUserStore, LiteDbUserStore>();
				services.AddTransient<IChatStore, LiteDbChatStore>();
				services.AddTransient<IChatParticipantStore, LiteDbChatParticipantStore>();
				services.AddTransient<IMessageStore, LiteDbMessageStore>();
				services.AddTransient<IUserUpdateStore, LiteDbUserUpdateStore>();
				services.AddTransient<IContactStore, LiteDbContactStore>();
				services.AddTransient<IPrivateChatDataStore, LiteDbPrivateChatStore>();

				services.AddTransient<LocalDbConfig>();
				services.AddTransient<ILocalDocumentStore, LocalDb>();
				services.AddTransient(typeof(ILocalDocumentStoreRepository<,>), typeof(LocalDbRepository<,>));

				services.AddTransient<PublicDbConfig>();
				services.AddTransient<IPublicDocumentStore, PublicDb>();
				services.AddTransient(typeof(IPublicDbRepository<,>), typeof(PublicDbRepository<,>));

				services.AddTransient<UserDbConfig>();
				services.AddTransient<IUserDocumentStore, UserDb>();
				services.AddTransient(typeof(IUserDocumentStoreRepository<,>), typeof(UserDbRepository<,>));
			}
			return services;
		}

		public static ISessionStore CreateSessionStore(this IMTServiceProvider provider)
		{
			return provider.GetService<ISessionStore>();
		}

	}
}
