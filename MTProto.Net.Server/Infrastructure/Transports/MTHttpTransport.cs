using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Requests.Help;
using MTProto.NET.Serializers;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MTProto.NET.Schema.MT;
using System.IO.Compression;
using MTProto.NET.Server.Infrastructure.Helpers;
using MTProto.NET.Server.Infrastructure.Implementations;
using MTProto.NET.Schema.Extra;
using MTProto.NET.Schema.TL.Auth;
using MTProto.NET.Schema.TL.Account;
using System.Threading;
using MTProto.NET.Schema.TL.Requests;

namespace MTProto.NET.Server.Infrastructure.Transports
{
	internal class InnerMessage
	{
		public int? max_delay;
		public int? max_wait;
		public int? wait_after;
		public ulong AuthKey { get; set; }

		public ulong sever_salt { get; set; }
		public ulong session_id { get; set; }
		public ulong container_msg_id { get; set; }
		public int container_seq_no { get; set; }
		public ulong msg_id { get; set; }
		public int seq_no { get; set; }
		public byte[] Content { get; set; }
		public InnerMessage DoClone(byte[] content = null, Action<InnerMessage> update = null)
		{
			var result = new InnerMessage
			{
				container_seq_no = this.container_seq_no,
				sever_salt = this.sever_salt,
				msg_id = this.msg_id,
				seq_no = this.seq_no,
				container_msg_id = this.container_msg_id,
				session_id = this.session_id,
				AuthKey = this.AuthKey,
				//Responses = this.Responses ?? new List<object>(),
				Content = content ?? this.Content

			};
			update?.Invoke(result);
			return result;
		}

	}
	public class MTHttpTransport : IMTTransport, IMiddleware, IMTService
	{
		private readonly ILogger<MTHttpTransport> logger;
		private readonly IMTServiceProvider serviceProvider;
		private readonly IMTSessionManager sessionManager;
		int? max_delay;
		int? max_wait;
		int? wait_after;
		public MTHttpTransport(ILogger<MTHttpTransport> logger, IMTServiceProvider serviceProvider, IMTSessionManager sessionManager)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			this.sessionManager = sessionManager;
		}
		private async Task<object> ProcessInitInvokeWithLayer(IMTSession session, InnerMessage message, CancellationToken cancellationToken)
		{
			var result = await Task.FromResult<object>(null);
			using (var stream = new MemoryStream(message.Content))
			using (var reader = new BinaryReader(stream))
			{

				var type = reader.ReadUInt32();
				var layer = reader.ReadInt32();
				var inner_message = reader.ReadAllBytes();
				this.logger.LogInformation("WithLayer...");
				result = await this.ProcessProtoMessage(session, message.DoClone(inner_message), cancellationToken);
			}

			return result;
		}
		private async Task<object> ProcessInitConnection(IMTSession session, InnerMessage message, CancellationToken cancellationToken)
		{
			var result = await Task.FromResult<object>(null);
			using (var stream = new MemoryStream(message.Content))
			using (var reader = new BinaryReader(stream))
			{
				var type = reader.ReadUInt32();
				var appId = reader.ReadUInt32();
				var device_info = StringSerializer.Deserialize(reader);
				var os_info = StringSerializer.Deserialize(reader);
				var app_version = StringSerializer.Deserialize(reader);
				var system_language = StringSerializer.Deserialize(reader);
				var system_language_pack = StringSerializer.Deserialize(reader);
				var lang_code = StringSerializer.Deserialize(reader);
				var remain = reader.ReadAllBytes();
				this.logger.LogInformation("Init...");
				result = await this.ProcessProtoMessage(session, message.DoClone(remain), cancellationToken);
			}
			return result;
		}
		private async Task<object> ProcessContainer(IMTSession session, InnerMessage message, CancellationToken cancellationToken)
		{
			var result = await Task.FromResult<object>(null);
			using (var stream = new MemoryStream(message.Content))
			using (var reader = new BinaryReader(stream))
			{
				this.logger.LogInformation("Container...");
				var type = reader.ReadUInt32();
				var count = reader.ReadInt32();
				for (var i = 0; i < count; i++)
				{
					var _message_id = reader.ReadUInt64();
					var _seq_no = reader.ReadInt32();
					var _len = reader.ReadInt32();
					var _content = reader.ReadBytes(_len);
					var _resp = await this.ProcessProtoMessage(session, message.DoClone(_content, m =>
					{
						m.msg_id = _message_id;
						m.seq_no = _seq_no;
					}), cancellationToken);
				}

			}
			return result;
		}

		private async Task<object> ProcessHttpWait(IMTSession session, InnerMessage message)
		{
			var result = await Task.FromResult<object>(null);
			var _req = session.Services.Serialization().Deserialize(message.Content) as MTHttpWait;
			if (_req != null)
			{
				this.max_delay = _req.MaxDelay;
				this.max_wait = _req.MaxWait;
				this.wait_after = _req.WaitAfter;
				message.max_wait = _req.MaxWait;
				message.max_delay = _req.MaxDelay;
				message.wait_after = _req.WaitAfter;
			}
			var _ret = new TLInputPeerEmpty
			{

			};
			if (_ret as MTObject != null)
			{
				result = new MTRpcResult
				{
					ReqMsgId = (long)message.msg_id,
					Result = new MTGzipPacked
					{
						PackedData = Utils.compress(BitConverter.GetBytes(0x997275b5))
					}
				};
				//session.AddMessage(result as MTObject, true, true);
			}

			return result;
		}
		private async Task<object> ProcessAck(IMTSession session, InnerMessage message)
		{
			var result = await Task.FromResult<object>(null);

			var ack = session.Services.Serialization().Deserialize(message.Content) as MTMsgsAck;
			var pendings = (await session.GetPending());
			var prev = pendings.Count();
			foreach (var msg_id in ack.MsgIds)
			{
				session.Ack(msg_id);
			}
			//session.AddMessage(new MTRpcResult
			//{
			//	ReqMsgId = (long)message.inner_msg_id,
			//	Result = new MTGzipPacked
			//	{
			//		PackedData = Utils.compress( new TLInputPeerEmpty { }.ToByteArray())
			//	}

			//}, false, true); 
			//(session as MTSession).GetMessageBox().Clear();
			this.logger.LogInformation(
				$"{prev - pendings.Count() } messages acknowledged");
			return result;



		}
		private async Task<object> ProcessMessage(IMTSession session, InnerMessage message, CancellationToken cancellationToken)
		{
			object result = null;
			ulong type = 0;
			try
			{
				type = BitConverter.ToUInt32(message.Content, 0);
				var req = message.Content.ToMTObject();// session.Services.Serialization().Deserialize(message.Content);
				{
					this.logger.LogInformation(req.GetType().Name);
					var envelop = MessageContext.Create(req);
					envelop.container_seq_no(message.container_seq_no);
					envelop.container_msg_id(message.container_msg_id);
					envelop.AuthKey(message.AuthKey);
					envelop.msg_id(message.msg_id);
					envelop.seq_no(message.seq_no);
					var _ret = await serviceProvider.Bus().Send(envelop, cancellationToken: cancellationToken) as MTObject;
					if (_ret as MTObject != null)
					{
						result = new MTRpcResult
						{
							ReqMsgId = (long)message.msg_id,
							Result = new MTGzipPacked
							{
								PackedData = Utils.compress(_ret.ToByteArray())
							}
						};
						session.AddMessage(result as MTObject, true, true);
					}
				}
			}
			catch (Exception err)
			{
				if (err is InvalidDataException)
				{
					this.logger.LogWarning(
						$"Invalid or Not Supported Constructor: '{type.ToString("X4")}'");
				}
				else
				{
					this.logger.LogError($"An error occured while trying to process this message: {err}");
				}
			}
			return result;
		}

		private async Task<object> ProcessAuthSendCode(IMTSession session, InnerMessage message)
		{
			var result = await Task.FromResult<object>(null);
			using (var stream = new MemoryStream(message.Content))
			using (var reader = new BinaryReader(stream))
			{
				var o = session.Services.Serialization().Deserialize(message.Content) as MTObject;
				//o.ToByteArray();

				//var type = reader.ReadUInt32();
				//var flags = reader.ReadUInt32();
				//var phone = StringSerializer.Deserialize(reader);
				///
				/// auth.sentCode#5e002502 flags:# type:auth.SentCodeType phone_code_hash:string next_type:flags.1?auth.CodeType timeout:flags.2?int = auth.SentCode;
				/// 
				this.logger.LogInformation("************************SendCode");
				var hash = new byte[8];
				new Random().NextBytes(hash);
				var code = "1234";
				//this.SentCode = true;
				var sent_code = new TLSentCode()
				{
					Flags = 4,
					PhoneCodeHash = "phonehash",
					Type = new TLSentCodeTypeSms
					{
						Length = 4,
					},
					Timeout = 40000

				};
				var ss = sent_code.ToByteArray();
				result = new MTRpcResult
				{
					ReqMsgId = (long)message.msg_id,
					Result = new MTGzipPacked
					{
						PackedData = Utils.compress(sent_code.ToByteArray())
					}
				};
				session.AddMessage(result as MTObject, false, true);
			}


			return result;



		}

		private async Task<object> ProcessInvokeAftert(IMTSession session, InnerMessage message, CancellationToken cancellationToken)
		{
			var result = await Task.FromResult<object>(null);
			var _req = session.Services.Serialization().Deserialize(message.Content) as TLInvokeAfterMsg;
			if (_req != null)
			{
				await ProcessProtoMessage(session, message.DoClone(_req.Query.ToByteArray(), cfg =>
				{
				}), cancellationToken);

			}
			return result;
		}

		private async Task<object> ProcessProtoMessage(IMTSession session, InnerMessage message, CancellationToken cancellationToken)
		{
			var type = BitConverter.ToUInt32(message.Content, 0);
			var result = await Task.FromResult<object>(null);
			try
			{
				switch (type)
				{
					case 0xcb9f372d:
						{
							// Invoke after Message
							await this.ProcessInvokeAftert(session, message, cancellationToken);
						}
						break;
					case 0x73f1f8dc:
						// container
						await this.ProcessContainer(session, message, cancellationToken);
						break;
					case 0xc7481da6:
						// init connection
						result = await this.ProcessInitConnection(session, message, cancellationToken);
						break;
					case 0xda9b0d0d:
						result = await this.ProcessInitInvokeWithLayer(session, message, cancellationToken);
						break;
					case 0x86aef0ec:
						result = await this.ProcessAuthSendCode(session, message);
						break;
					case 0x62d6b459:
						result = await this.ProcessAck(session, message);
						break;
					case 0x9299359f:
						/// Http Wait
						/// 
						{
							await this.ProcessHttpWait(session, message);
						}
						break;
					default:
						await this.ProcessMessage(session, message, cancellationToken);
						break;
				}
			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"===============================================>Unknown Request: '{type.ToString("X4")}'");
			}
			return result;
		}

		public static int POSTED;
		private async Task<Tuple<int, byte[]>> GenerateResponse(IMTSession session)
		{

			byte[] repl = new byte[] { };
			var pendings = (await session.GetPending()).ToArray();
			if (session.GetUserId() == 2 && POSTED > 0)
			{
				(await session.GetPending()).ToArray();

			}
			var count = pendings.Count();
			//this.logger.LogInformation($"*** Generating Responses Pending count: {pendings.Length}");
			using (var memory = new MemoryStream())
			using (var w = new BinaryWriter(memory))
			{
				w.Write(0x73f1f8dc); //Message Container
				w.Write(pendings.Length);
				foreach (var item in pendings)
				{
					using (var _memory = new MemoryStream())
					using (var _w = new BinaryWriter(_memory))
					{
						_w.Write(item.MsgId);
						_w.Write(item.Seqno);
						_w.Write(item.Bytes);
						_w.Write(item.Body.PackedData);
						_w.Close();
						w.Write(_memory.ToArray());
					}
				}
				w.Close();
				if (session.GetUserId() == 2 && pendings.Count() > 1)
				{

				}


				repl = await Task.FromResult(memory.ToArray());
				return new Tuple<int, byte[]>(count, repl);
			}
		}

		private async Task HandlePlainText(HttpContext context, ulong auth_key, BinaryReader reader)
		{
			var message_id = reader.ReadUInt64();
			var message_size = reader.ReadInt32();
			var envelop = MessageContext.Create(reader.ReadMTObject());
			envelop.AuthKey(auth_key);
			envelop.MessageId(message_id);
			envelop.MessageSize(message_size);
			this.logger.LogDebug(
				"PlainText request received: {0}", envelop.Body);

			var response = await MTServer.Instance.Services.Bus().Send(envelop);
			var writer = new BinaryWriter(context.Response.Body);
			writer.Write(auth_key);
			writer.Write(message_id);
			writer.Write(message_size);
			if (response != null)
			{
				MTObjectSerializer.Serialize(response as MTObject, writer);
				this.logger.LogDebug(
						$"PlainText request successfully handled. Response: '{response}'");
			}
			else
			{
				this.logger.LogWarning(
					$"No response for: '{envelop.Body}'");
				//context.Response.StatusCode = 404;
			}
			writer.Close();

		}
		private async Task HandleEncryptedRequest(HttpContext context, ulong auth_key, BinaryReader reader)
		{
			var message_key = reader.ReadBytes(128 / 8);
			var session = this.sessionManager.GetSession(auth_key);

			if (session != null)
			{
				var message = session.Decrypt(message_key, reader.ReadAllBytes());
				using (var s = new MemoryStream(message))
				using (var r = new BinaryReader(s))
				{

					var sever_salt = r.ReadUInt64();
					var session_id = r.ReadUInt64();
					var msg_id = r.ReadUInt64();
					var seq_no = r.ReadInt32();
					var len = r.ReadInt32();
					var inner_message = new InnerMessage
					{
						container_msg_id = msg_id,
						container_seq_no = seq_no,
						session_id = session_id,
						sever_salt = sever_salt,
						AuthKey = auth_key,
						Content = r.ReadAllBytes(),
					};
					await session.KeepAlive(session_id);
					//session.op

					await this.ProcessProtoMessage(session, inner_message, context.RequestAborted);

					/// Http Wait 
					/// 

					await Task.Delay(this.max_delay ?? 100);
					var pendings = (await session.GetPending()).ToArray();
					if (pendings.Length == 0)
					{
						await Task.Delay(this.max_wait ?? 2500);

						var wait = this.max_wait ?? 2500;
						this.logger.LogDebug($"Http Wait ({wait}).....");
						for (int i = 0; i < wait; i = i + 100)
						{
							try
							{
								await Task.Delay(100, context.RequestAborted);
								pendings = (await session.GetPending()).ToArray();
							}
							catch { }
							if (pendings.Length > 0)
								break;
							if (context.RequestAborted.IsCancellationRequested)
								break;
						}
					}
					var response = await this.GenerateResponse(session);
					session.TryCreateResponseMessage(response.Item2, inner_message.sever_salt, inner_message.session_id, out var _encr, out var msgkey, true);
					using (var memory = new MemoryStream())
					using (var w = new BinaryWriter(memory))
					{
						w.Write(session.AuthId());
						w.Write(msgkey);
						w.Write(_encr);
						w.Flush();
						w.Close();
						context.Response.Body.Write(memory.ToArray(), 0, memory.ToArray().Length);
						this.logger.LogInformation($"Response Sent: Count:'{response.Item1}', Length:'{response.Item2.Length}'");
					}
				};
			}
			else
			{
				throw new Exception(
					$"Session Not Found. AuthKeyId:{auth_key}");

			}

		}

		//public async Task HandleRequest(HttpContext context)
		//{
		//	this.logger.LogInformation("Middleware InvokeAsync Starts");
		//	var reader = new MTStreamReader(context.Request.Body);
		//	var auth_key = reader.ReadUInt64();
		//	try
		//	{
		//		if (auth_key == 0)
		//		{
		//			await this.HandlePlainText(context, auth_key, reader);
		//			if (1 == 0)
		//			{
		//				// Plain Text Message
		//				var message_id = reader.ReadUInt64();
		//				var message_size = reader.ReadInt32();
		//				var envelop = MessageContext.Create(reader.ReadObject());
		//				envelop.AuthKey(auth_key);
		//				envelop.MessageId(message_id);
		//				envelop.MessageSize(message_size);
		//				this.logger.LogDebug(
		//					"PlainText request received: {0}", envelop.Body);

		//				var response = await MTServer.Instance.Services.Bus().Send(envelop);
		//				var writer = new BinaryWriter(context.Response.Body);
		//				writer.Write(auth_key);
		//				writer.Write(message_id);
		//				writer.Write(message_size);
		//				if (response != null)
		//				{
		//					MTObjectSerializer.Serialize(response as MTObject, writer);
		//					this.logger.LogDebug(
		//							$"PlainText request successfully handled. Response: '{response}'");
		//				}
		//				else
		//				{
		//					this.logger.LogWarning(
		//						$"No response for: '{envelop.Body}'");
		//					//context.Response.StatusCode = 404;
		//				}
		//				writer.Close();
		//			}
		//		}
		//		else
		//		{
		//			await this.HandleEncryptedRequest(context, auth_key, reader);
		//			if (1 == 0)
		//			{
		//				// Encrypted Message
		//				var message_key = reader.ReadBytes(128 / 8);
		//				var session = this.sessionManager.GetSession(auth_key);
		//				if (session != null)
		//				{
		//					var message = session.Decrypt(message_key, reader.ReadAllBytes());
		//					using (var s = new MemoryStream(message))
		//					using (var r = new BinaryReader(s))
		//					{

		//						var sever_salt = r.ReadUInt64();
		//						var session_id = r.ReadUInt64();
		//						var msg_id = r.ReadUInt64();
		//						var seq_no = r.ReadInt32();
		//						var len = r.ReadInt32();
		//						var inner_message = new InnerMessage
		//						{
		//							container_msg_id = msg_id,
		//							container_seq_no = seq_no,
		//							session_id = session_id,
		//							sever_salt = sever_salt,
		//							Content = r.ReadAllBytes(),
		//						};

		//						//var message_context = MessageContext.Create(inner_message.Content);
		//						//message_context.seq_no(inner_message.seq_no);
		//						//message_context.MessageId(inner_message.msg_id);


		//						var _request = await this.ProcessProtoMessage(session, inner_message);

		//						/// Http Wait 
		//						/// 

		//						if (1 == 1)
		//						{
		//							await Task.Delay(this.max_delay ?? 100);

		//							if (session.GetPending().Count() == 0)
		//							{
		//								this.logger.LogInformation("Waitin.......................");
		//								await Task.Delay(this.max_wait ?? 2500);
		//							}
		//							var response_bytes = await this.GenerateResponse(session);
		//							session.TryCreateResponseMessage(response_bytes, inner_message.sever_salt, inner_message.session_id, out var _encr, out var msgkey, true);
		//							//session.GGG(_result.ToByteArray(), inner_message.sever_salt, inner_message.session_id, out var _encr, out var msgkey);
		//							using (var memory = new MemoryStream())
		//							using (var w = new BinaryWriter(memory))
		//							{
		//								w.Write(session.AuthId());
		//								w.Write(msgkey);
		//								w.Write(_encr);
		//								w.Flush();
		//								w.Close();
		//								context.Response.Body.Write(memory.ToArray(), 0, memory.ToArray().Length);
		//								this.logger.LogInformation("XXXX Response sent...");
		//							}

		//						}




		//					};
		//				}
		//				else
		//				{

		//				}
		//			}

		//		}
		//	}
		//	catch (Exception err)
		//	{
		//		this.logger.LogError(
		//			$"An error occured while trying to handle request :{err}");
		//	}

		//}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (context != null && context.IsMTPPacket())
			{
				this.logger.LogTrace("Middleware InvokeAsync Starts");
				try
				{
					var reader = new MTStreamReader(context.Request.Body);
					var auth_key = reader.ReadUInt64();
					if (auth_key == 0)
					{
						await this.HandlePlainText(context, auth_key, reader);
					}
					else
					{
						await this.HandleEncryptedRequest(context, auth_key, reader);
					}
					this.logger.LogTrace("Middleware InvokeAsync Successfully Handled.");
				}
				catch (Exception err)
				{
					this.logger.LogError(
						$"An error occured while trying to handle request :{err}");
				}
			}
			else
			{
				await next(context);
			}
		}
	}

}

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		public static int? container_seq_no(this IMessageContext ctx, int? value)
		{
			if (value.HasValue)
			{
				ctx.Headers.AddOrUpdateValue<int?>("seq_no", value.Value);
			}
			return ctx.Headers.TryGetValue<int?>("seq_no", out var v) ? v : null;
		}
		public static int? seq_no(this IMessageContext ctx, int? value)
		{
			if (value.HasValue)
			{
				ctx.Headers.AddOrUpdateValue<int?>("inner_seq_no", value.Value);
			}
			return ctx.Headers.TryGetValue<int?>("inner_seq_no", out var v) ? v : null;
		}
		public static ulong? container_msg_id(this IMessageContext ctx, ulong? value)
		{
			if (value.HasValue)
			{
				ctx.Headers.AddOrUpdateValue<ulong?>("msg_id", value.Value);
			}
			return ctx.Headers.TryGetValue<ulong?>("msg_id", out var v) ? v : null;
		}
		public static ulong? msg_id(this IMessageContext ctx, ulong? value)
		{
			if (value.HasValue)
			{
				ctx.Headers.AddOrUpdateValue<ulong?>("inner_msg_id", value.Value);
			}
			return ctx.Headers.TryGetValue<ulong?>("inner_msg_id", out var v) ? v : null;
		}

		public static ulong? AuthKey(this IMessageContext envelop, ulong? value = null)
		{
			if (value.HasValue)
			{
				envelop.Headers.AddOrUpdateValue<ulong?>("auth_key", value.Value);
			}
			return envelop.Headers.TryGetValue<ulong?>("auth_key", out var v) ? v : null;
		}

		public static ulong? MessageId(this IMessageContext envelop, ulong? value)
		{
			if (value.HasValue)
			{
				envelop.Headers.AddOrUpdateValue<ulong?>("message_id", value.Value);
			}
			return envelop.Headers.TryGetValue<ulong?>("message_id", out var v) ? v : null;
		}
		public static int? MessageSize(this IMessageContext envelop, int? value)
		{
			if (value.HasValue)
			{
				envelop.Headers.AddOrUpdateValue<int?>("message_size", value.Value);
			}
			return envelop.Headers.TryGetValue<int?>("message_size", out var v) ? v : null;
		}

		public static byte[] Message_Key(this IMessageContext envelop, byte[] value = null)
		{
			if (value != null)
			{
				envelop.Headers.AddOrUpdateValue<byte[]>("message_key", value);
			}
			return envelop.Headers.TryGetValue<byte[]>("message_key", out var v) ? v : null;
		}
	}
}

