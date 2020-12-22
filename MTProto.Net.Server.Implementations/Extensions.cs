using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Server.Implementations.Messages;
using MTProto.NET.Server.Implementations.Updates;
using MTProto.NET.Server.Infrastructure;
using System;

namespace MTProto.NET.Server.Implementations
{
	public static partial class  Extensions
	{
		public static IMTServerBuilder AddMessages(this IMTServerBuilder builder)
		{
			//IServiceCollection i = null;
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, GetDialogsHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, GetHistoryHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, SendMessageHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, GetStateHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, ReadHistoryHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, ReceivedMessagesHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, Account.UpdateProfileHandler>();

			/// Contact
			/// 
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, Contacts.ImportContactsHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, Contacts.GetContactsHandler>();

			// Upload
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, Upload.SaveFilePartHandler>();
			builder.ServiceCollection.AddTransient<IProtoMessageHanddler, Photos.UploadProfilePhotoHandler>();

			return builder;
		}
	}
}
