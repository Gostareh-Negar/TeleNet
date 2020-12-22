using Microsoft.AspNetCore.Http;
using MTProto.NET;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TDLib.Server2;
using TLSharp.Core.MTProto.Crypto;

namespace TelegramServer.Handlers
{
	public interface IMTProtoContext
	{
		//IDictionary<string, string> Headers { get; }
		BinaryReader Reader { get; }
		BinaryWriter Writer { get; }
		MTObject Request { get; }

		Task Handle();
		//ISessionManager SessionManager { get; }
		IMTProtoSession Session { get; }
	}

	public class MTProtoContext : IMTProtoContext
	{
		public BinaryReader Reader { get; private set; }
		public BinaryWriter Writer { get; private set; }
		public MTObject Request { get; private set; }
		public MTObject Response { get; private set; }
		//public ISessionManager Sessions { get; private set; }
		public IMTProtoSession Session { get; private set; }
		public ulong AuthKey;
		public ulong Key;
		public int Size;
		public HttpContext HttpContext { get; private set; }
		public static byte[] ReadAllBytes(BinaryReader reader)
		{
			const int bufferSize = 4096;
			using (var ms = new MemoryStream())
			{
				byte[] buffer = new byte[bufferSize];
				int count;
				while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
					ms.Write(buffer, 0, count);
				return ms.ToArray();
			}

		}
		public void ProcessMessageContainer(BinaryReader reader)
		{
			var count = reader.ReadInt32();
			for(var i = 0; i < count; i++)
			{
				var message_id = reader.ReadUInt64();
				var seq_no = reader.ReadUInt32();
				var len = reader.ReadInt32();
				var content = reader.ReadBytes(len);
				var strm = new MemoryStream(content);
				var r = new BinaryReader(strm);
				var tt = r.ReadUInt32();


			}
		}
		public void StartEncrypted()
		{
			var msg_key = this.Reader.ReadBytes(128 / 8);
			//var auth_key = new BigInteger(this.Session.GetValue<string>("Auth_Key"));
			var k = "15a403da43711c9579e19666a1bacc0e23b00ac64a2245a44b962f9cd3544882f18042ffebad9f6083d761eae728334ad0b313fe6f9db39e317dfa2707b5c6372f49333dab3e04921422bb8f1b92cc8c27ff86a289d7823d5b807148a3e045ee87dae8e8bd082c3d6ebb8f0c668b8162585cfa7f54c8e5ce1b3caee988f518788aa86274b174253e2d99909474bdfe45ca53538ca907646893dabcdae87cfec599d2dd662996e2c828acfd33ff4cd826a2885fbc84a531fd7077f6e86c18259838f02004c05c72e5139ca9ab112e2516560bf3953d557cc9414184724d0fa61cbfeb28c796e00ef707d5ade0191a12d19b417b8cfaed680e780927bd53ea573c";
			try
			{
				var auth_key = RsaKey.StringToByteArray(k);
				var ivs = TLSharp.Core.Utils.Helpers.CalcKey_2(auth_key, msg_key, true);
				var bytes = ReadAllBytes(Reader);

				var ff = AES.DecryptAES(ivs, bytes);
				using (var s = new MemoryStream(ff))
				using (var r = new BinaryReader(s))
				{

					var sever_salt = r.ReadUInt64();
					var session_id = r.ReadUInt64();
					var msg_id = r.ReadUInt64();
					var seq_no = r.ReadInt32();
					var len = r.ReadInt32();

					var msg_type = r.ReadUInt32();
					switch (msg_type)
					{
						case 0x73f1f8dc:
							ProcessMessageContainer(r);
							break;
						default:
							break;
					}
					//var obj = MTObjectSerializer.Deserialize(r);



				};
			}
			catch (Exception err)
			{

			}

		}
		public MTProtoContext(HttpContext context)//BinaryReader reader, BinaryWriter writer)
		{
			this.HttpContext = context;
			this.Session = SessionManager.Instance.GetSession(context.Connection.Id, cfg => { });
			if (this.Session.TryGetValue<string>("Auth_Key", out var auth))
			{
				Console.WriteLine(auth);
				//this.Session.Headers.TryAdd("babak", "babak");
			}
			var reader = new BinaryReader(context.Request.Body);
			var writer = new BinaryWriter(context.Response.Body);
			this.Reader = reader;
			this.Writer = writer;
			this.AuthKey = reader.ReadUInt64();
			if (this.AuthKey != 0)
			{

				StartEncrypted();

			}
			this.Key = reader.ReadUInt64();
			this.Size = reader.ReadInt32();
			try
			{
				this.Request = MTObjectSerializer.Deserialize(reader);
			}
			catch (Exception err)
			{

			}

		}

		public async Task Handle()
		{
			MTObject response = null;
			switch (this.Request)
			{
				case MTReqPq hh:
					{
						var handler = new MTReqPqHandler();
						response = await handler.Handle(this);
					}
					//res = await handler.HandleEx(hh);
					break;
				case MTReqDhParams hh:
					{
						var handler = new ReqDHParamsHandler();
						response = await handler.Handle(this);
					}
					break;
				case MTSetClientDhParams req:
					{
						var handler = new SetClientDHParamsHandler();
						response = await handler.Handle(this);

					}
					break;
				default:
					response = null;
					break;
			}
			this.Response = response;
			this.Writer.Write(this.AuthKey);
			this.Writer.Write(this.Key);
			this.Writer.Write(this.Size);
			if (response != null)
				MTObjectSerializer.Serialize(response as MTObject, this.Writer);
		}
	}
}
