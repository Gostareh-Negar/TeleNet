using MTProto.NET.Schema.Layer72;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IChatManager
	{
		/// <summary>
		/// Gets user's chats. 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		Task<IEnumerable<IChat>> GetUserChats(int userId, int offset = 0, int count = 30);


		/// <summary>
		/// Gets a messages of a private chat.
		/// </summary>
		/// <param name="fromUserId"></param>
		/// <param name="userPeerId"></param>
		/// <returns></returns>
		Task<IEnumerable<IMessage>> GetPrivateChatMessages(int fromUserId, int userPeerId);

		/// <summary>
		/// Registers a message in a private chat.
		/// </summary>
		/// <param name="fromUserId"></param>
		/// <param name="toUserPeerId"></param>
		/// <returns></returns>
		Task<IMessage> SendPrivateChatMessage(int fromUserId,int peerUserId,TLSendMessage message);
	}
}
