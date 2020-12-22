using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IMTObjectFactory
	{
		IMessage CreateMessage(IMessageData data);
		IMessageData CreateMessageData();

		IUser CreateUser(IUserData userData);
		IUserUpdateData CreateUserUpdate();
	}
}
