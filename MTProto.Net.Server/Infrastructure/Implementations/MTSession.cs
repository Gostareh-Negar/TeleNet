using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.MT;
using MTProto.NET.Server.Contracts.Authorization;
using MTProto.NET.Server.Infrastructure.Configurations;
using MTProto.NET.Server.Infrastructure.Encryption;
using MTProto.NET.Server.Infrastructure.Encryption.Helpers.Legacy;
using MTProto.NET.Server.Infrastructure.Helpers;
using MTProto.NET.Server.Infrastructure.Storage;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class MTSession : IMTSession, IAuthorizationSession
	{
		private readonly ILogger<MTSession> logger;
		private readonly ServerOptions options;
		private SessionData data;
		private IRSAKey serverKey;
		private ulong lastMessageId;
		private SessionMessageQueue messageQueue;
		private IUserManager userManager;

		public DateTime? LastActivityOn { get; private set; }
		public IUpdateManager UpdateManager { get; private set; }

		public SessionData Data => this.data;
		public IMTServiceProvider Services { get; private set; }

		public IChatManager ChatManager { get; private set; }

		public IUserManager UserManager => this.userManager;

		public MTSession(ILogger<MTSession> logger, ServerOptions options, IMTServiceProvider serviceProvider)
		{
			this.logger = logger;
			this.options = options;
			this.serverKey = options.ServerKey;
			this.Services = serviceProvider;
			this.messageQueue = new SessionMessageQueue(this);
			this.userManager = serviceProvider.GetService<IUserManager>();
			this.ChatManager = serviceProvider.GetService<IChatManager>();
			this.UpdateManager = serviceProvider.GetService<IUpdateManager>();

		}

		private SessionMessageQueue GetMessageQueue()
		{
			if (this.messageQueue == null)
			{
				this.messageQueue = new SessionMessageQueue(this);
			}
			return this.messageQueue;
		}

		public byte[] GetFingerPrint()
		{
			return this.options.ServerKey.GetFingerPrints();
		}

		public IMTSession Initialize(SessionData sessionData)
		{
			this.data = sessionData;
			return this;
		}

		public ulong m_pq()
		{
			return this.options.pq;
		}

		public BigInteger ServerOnce()
		{
			return this.options.GetNonce();
		}

		public void Update()
		{
			this.Data?.Upsert();
		}

		public BigInteger CreateDh_ma()
		{
			var m_a = new byte[256];
			new Random().NextBytes(m_a);
			var result = new BigInteger(1, m_a);
			this.data.Dh_ma = result.ToString();
			return result;
		}


		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}
		public byte[] DhGetPrime()
		{
			return StringToByteArray(this.options.ServerKey.DhPrime);

		}

		public int Dh_mg()
		{
			return 3;
		}
		public BigInteger NewDh_ma()
		{
			var m_a = new byte[256];
			new Random().NextBytes(m_a);
			var result = new BigInteger(1, m_a);
			return result;
		}



		public byte[] DecryptClientMessage(byte[] encriptedData)
		{
			var big1 = new BigInteger(encriptedData);
			var par = this.options.ServerKey.GetPrivateKeyParams();
			var big4 = big1.ModPow(new BigInteger(1, par.D), new BigInteger(1, par.Modulus)).ToByteArrayUnsigned();
			return big4.Skip(20).ToArray();

		}

		public byte[] EncryptWithServerNonce(byte[] serverNonce, byte[] newNonce, byte[] data)
		{
			var key = AES.GenerateKeyDataFromNonces(serverNonce, newNonce);
			return AES.EncryptAES(key, data);
		}
		public byte[] DecryptWithNonces(byte[] data, byte[] serverNonce, byte[] newNonce)
		{
			return AES.DecryptWithNonces(data, serverNonce, newNonce);
		}

		public byte[] Decrypt(byte[] messageId, byte[] data)
		{
			var auth_key = new BigInteger(this.data.AuthKey, 16).ToByteArray();
			var ivs = MTProto.NET.Server.Infrastructure.Encryption.Helpers.Legacy.Helpers.CalcKey_2(auth_key, messageId, true);
			return AES.DecryptAES(ivs, data);

		}

		public uint GetNextMessageSequenceNumber(bool isContent)
		{
			return Utils.GetNewMessageSequenceNumber(this.data.ContentRelatedMessageNumber, isContent);
		}

		public ulong NewMessageId(bool isReply = true)
		{
			this.lastMessageId = Utils.CreateNewMessageId(this.lastMessageId, isReply);
			return this.lastMessageId;
		}

		public byte[] GetAuthKey()
		{
			return this.data.GetAuthKeyAsByteArray();
		}
		public byte[] GetServerKeyPart()
		{

			return this.data.GetServerKeyPart();
		}
		public byte[] GetClientKeyPart()
		{

			return this.data.GetClientKeyPart();
		}

		public ulong GetServerSalt()
		{
			throw new NotImplementedException();
		}

		public ulong SessionId()
		{
			throw new NotImplementedException();
		}

		public ulong AuthId()
		{
			return this.data.AuthKeyId;
		}


		public bool TryCreateResponseMessage(MTObject resp, ulong server_salt, ulong session_id, out byte[] encr, out byte[] messagekey, bool encrypt)
		{
			return TryCreateResponseMessage(resp.ToByteArray(), server_salt, session_id, out encr, out messagekey, encrypt);

			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				int c_alignment = 16;
				int c_v2_minimumPadding = 12;
				/// Write Header
				/// 
				//writer.Write(session.GetServerSalt());
				writer.Write(server_salt);
				writer.Write(session_id);
				//writer.Write(session.SessionId());
				var id = NewMessageId(true);
				writer.Write(id);
				var seq = GetNextMessageSequenceNumber(true);
				writer.Write(seq);
				var content = resp.ToByteArray();
				writer.Write(content.Length);
				writer.Write(content);
				var packageLength = writer.BaseStream.Length;


#if USE_MTProto_V1
#else
				var padding = c_alignment - (packageLength % c_alignment);
				if (padding < c_v2_minimumPadding)
				{
					padding = padding += c_alignment;
				}
				if (padding > 0)
				{
					var paddingbytes = new byte[padding];
					new Random().NextBytes(paddingbytes);
					writer.Write(paddingbytes);
				}
				var key = GetServerKeyPart();
				writer.Flush();
				writer.Close();
				var decryptedData = stream.ToArray();
				messagekey = SHA256.Create().ComputeHash(key.Concat(decryptedData).ToArray()).Skip(8).Take(16).ToArray();
				var keys = MTProto.NET.Server.Infrastructure.Encryption.Helpers.Legacy.Helpers.CalcKey_2(GetAuthKey(), messagekey, false);
				var encrypted = MTProto.NET.Server.Infrastructure.Encryption.Helpers.Legacy.AES.EncryptAES(keys, decryptedData);
				if (encrypt)
				{
					encr = encrypted;
				}
				else
				{
					encr = decryptedData;
				}

				return true;







#endif



			}
		}

		public bool TryCreateResponseMessage(byte[] resp, ulong server_salt, ulong session_id, out byte[] encr, out byte[] messagekey, bool encrypt)
		{

			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				int c_alignment = 16;
				int c_v2_minimumPadding = 12;
				writer.Write(server_salt);
				writer.Write(session_id);
				//writer.Write(session.SessionId());
				var id = NewMessageId(true);
				writer.Write(id);
				var seq = GetNextMessageSequenceNumber(true);
				writer.Write(seq);
				var content = resp;
				writer.Write(content.Length);
				writer.Write(content);
				var packageLength = writer.BaseStream.Length;


#if USE_MTProto_V1
#else
				var padding = c_alignment - (packageLength % c_alignment);
				if (padding < c_v2_minimumPadding)
				{
					padding = padding += c_alignment;
				}
				if (padding > 0)
				{
					var paddingbytes = new byte[padding];
					new Random().NextBytes(paddingbytes);
					writer.Write(paddingbytes);
				}
				var key = GetServerKeyPart();
				writer.Flush();
				writer.Close();
				var decryptedData = stream.ToArray();
				messagekey = SHA256.Create().ComputeHash(key.Concat(decryptedData).ToArray()).Skip(8).Take(16).ToArray();
				var keys = MTProto.NET.Server.Infrastructure.Encryption.Helpers.Legacy.Helpers.CalcKey_2(GetAuthKey(), messagekey, false);
				var encrypted = MTProto.NET.Server.Infrastructure.Encryption.Helpers.Legacy.AES.EncryptAES(keys, decryptedData);
				if (encrypt)
				{
					encr = encrypted;
				}
				else
				{
					encr = decryptedData;
				}
				return true;







#endif



			}
		}

		public TransportMessage AddMessage(MTObject message, bool isContentRelated, bool isReply)
		{
			var msg_id = (long)this.NewMessageId(isReply);
			var seq = (int)this.GetNextMessageSequenceNumber(isContentRelated);
			byte[] content = new byte[] { };
			if (message.GetType() == typeof(MTRpcResult))
			{
				content = message.ToByteArray();
			}
			else
			{
				content = message.ToByteArray();// Utils.compress(message.ToByteArray());

			}
			var _message = new TransportMessage
			{
				MsgId = msg_id,
				Seqno = seq,
				Bytes = content.Length,
				Body = new MTGzipPacked
				{
					PackedData = content
				}
			};
			this.GetMessageQueue().AddMessage(_message);
			return _message;
		}

		private List<NET.Schema.TL.TLUpdates> updates = new List<NET.Schema.TL.TLUpdates>();
		public async Task<IEnumerable<TransportMessage>> GetPending()
		{
			var result = this.GetMessageQueue().GetPending().ToList();
			var updates = await this.UpdateManager.GetPedningUpdate(this.GetUserId());
			if (updates.Updates.Count > 0)
			{
				var msg_id = (long)this.NewMessageId(false);
				var seq = (int)this.GetNextMessageSequenceNumber(false);
				var content = updates.ToByteArray();
				var _message = new TransportMessage
				{
					MsgId = msg_id,
					Seqno = seq,
					Bytes = content.Length,
					Body = new MTGzipPacked
					{
						PackedData = content
					}
				};
				result.Add(_message);
			}
			return result;

		}

		public void Ack(long msgId)
		{
			this.GetMessageQueue().Ack(msgId);
		}

		public Task<IUser> GetUser()
		{
			return this.UserManager.GetUserById(this.GetUserId());
		}

		public async Task<IUser> SetUser(string mobilePhone)
		{
			IUser user = null;
			var helper = Extensions.GetMobilePhoneHelper(mobilePhone);
			try
			{
				if (!helper.IsValid)
					throw new Exception($"Invalid Phone. '{mobilePhone}' is not a valid mobile phone number");

				user = await this.userManager.GetUserByMobilePhone(mobilePhone);
				if (user == null)
				{
					user = await this.userManager.CreateUser(mobilePhone, (long)this.AuthId());
				}
				if (user == null)
					throw new Exception(
						$"Failed to create user.");
				this.data.UserId = user.Id;
				this.Update();

			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"aAn error occured while trying to set user of this session: \r\n{err.GetBaseException().Message}");
				throw;
			}
			return user;
		}

		public int GetUserId()
		{
			return this.Data.UserId;
		}

		public Task PostUpdate(NET.Schema.TL.TLUpdates updates)
		{
			this.updates.Add(updates);


			return Task.CompletedTask;
			var msg_id = (long)this.NewMessageId(false);
			var seq = (int)this.GetNextMessageSequenceNumber(false);
			var content = Utils.compress(updates.ToByteArray());
			var _message = new TransportMessage
			{
				MsgId = msg_id,
				Seqno = seq,
				Bytes = content.Length,
				Body = new MTGzipPacked
				{
					PackedData = content
				}
			};
			SessionMessageQueue.MSGID = msg_id;
			this.GetMessageQueue().AddMessage(_message);
			return Task.CompletedTask;
		}

		public async Task SetAuthKey(BigInteger authKey, ulong authKeId)
		{
			if (this.data == null)
				throw new Exception($"Unexpected. Data is null");
			this.data.SetAuthKey(authKey);
			this.data.AuthKeyId = authKeId;
			this.Update();
			await MTServer.Bus.Publish(new NewAuthorizationKey { KeyId = authKeId });
		}

		public async Task Bind(ulong sessionId)
		{
			if (this.data.SessionId != sessionId)
			{
				if (this.data.SessionId != 0 && this.data.SessionId != sessionId)
				{
					//this.logger.LogWarning(
					//	$"AuthKeyId already binded in a different session. Probably we should raise an error here. Prev Session Id:{this.data.SessionId}, SessionId:{sessionId}, AuthKeyId:{this.data.AuthKeyId} ");
				}
				this.data.SessionId = sessionId;
				//Update();
				await MTServer.Bus.Publish(new Contracts.Authorization.NewSessionCreated
				{
					SessionId = sessionId,
					AuthKeyId = this.data.AuthKeyId
				});
			}
		}
		public async Task KeepAlive(ulong sessionId)
		{
			await Bind(sessionId);
			this.LastActivityOn = DateTime.UtcNow;

		}
	}
}
