using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IStore
	{
		IMTServiceProvider ServiceProvider { get; }
		IPrivateChatDataStore GetPrivateChatDataStore();
		IUserStore GetUserStore();
		IUserUpdateStore GetUserUpdateStore();
		IContactStore GetContactSTore();
		IMessageStore GetMessageStore();
		//IPrivateChatDataStore GetPrivateChatStore();
	}
}
