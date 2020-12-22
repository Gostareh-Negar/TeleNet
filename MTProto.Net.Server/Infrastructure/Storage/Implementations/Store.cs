using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations
{
	class Store : IStore
	{
		public IMTServiceProvider ServiceProvider { get; set; }

		
		public Store(IMTServiceProvider provider)
		{
			this.ServiceProvider = provider;
		}

		public IPrivateChatDataStore GetPrivateChatDataStore()
		{
			return this.ServiceProvider.GetService<IPrivateChatDataStore>();
		}

		public IUserStore GetUserStore()
		{
			return this.ServiceProvider.GetService<IUserStore>();
		}

		public IUserUpdateStore GetUserUpdateStore()
		{
			return this.ServiceProvider.GetService<IUserUpdateStore>();
		}

		public IContactStore GetContactSTore()
		{
			return this.ServiceProvider.GetService<IContactStore>();
		}

		public IPrivateChatDataStore GetPrivateChatStore()
		{
			return this.ServiceProvider.GetService<IPrivateChatDataStore>();
		}

		public IMessageStore GetMessageStore()
		{
			return this.ServiceProvider.GetService<IMessageStore>();
		}
	}
}
