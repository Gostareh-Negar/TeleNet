using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class MTObjectFactory : IMTObjectFactory
	{
		public IMTServiceProvider ServiceProvider { get; private set; }
		public MTObjectFactory(IMTServiceProvider serviceProvider)
		{

		}

		public IMessage CreateMessage(IMessageData data)
		{
			return new Message(data);
		}

		public IMessageData CreateMessageData()
		{
			return new MessageData();
		}

		public IUser CreateUser(IUserData userData)
		{
			return new User(userData);
		}

		public IUserUpdateData CreateUserUpdate()
		{
			return new UserUpdateData();
		}
	}
}
