using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.TL;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure
{
	
	public interface IMTSession
	{
		//IMessageBox MessageBox { get; }
		SessionData Data { get; }
		IMTServiceProvider Services { get; }
		IChatManager ChatManager { get; }
		IUserManager UserManager { get; }
		IUpdateManager UpdateManager { get; }
		IMTSession Initialize(SessionData sessionData);
		DateTime? LastActivityOn { get; }
		byte[] Decrypt(byte[] messageId, byte[] data);
		uint GetNextMessageSequenceNumber(bool isContent);
		ulong NewMessageId(bool isReply = true);
		ulong GetServerSalt();
		ulong SessionId();
		ulong AuthId();
		byte[] GetAuthKey();
		byte[] GetServerKeyPart();
		byte[] GetClientKeyPart();

		bool TryCreateResponseMessage(MTObject resp, ulong server_salt, ulong session_id, out byte[] encr, out byte[] message_key, bool encrypt);
		bool TryCreateResponseMessage(byte[] resp, ulong server_salt, ulong session_id, out byte[] encr, out byte[] messagekey, bool encrypt);

		TransportMessage AddMessage(MTObject message, bool isContentRelated, bool isReply);

		Task<IEnumerable<TransportMessage>> GetPending();

		void Ack(long msgId);

		Task<IUser> GetUser();

		int GetUserId();
		/// <summary>
		/// Sets the user of this session based on given mobile phone.
		/// This may create a user if the mobile phone is not yet registered.
		/// This is called in SignIn process. 
		/// </summary>
		/// <param name="mobilePhone"></param>
		/// <returns></returns>
		Task<IUser> SetUser(string mobilePhone);

		Task PostUpdate(TLUpdates updates);

		Task KeepAlive(ulong sessionId);
		

		
	}

}
