using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MTProto.NET;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Serializers;
using Org.BouncyCastle.Math;
using TDLib.Server2;
using TelegramServer.Handlers;

namespace TelegramServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddControllers();
			services.AddCors(cfg =>
			{
				cfg.AddPolicy("cors", p =>
				{
					p.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
				});
			});
		}

		public BigInteger ServerOnce()
		{
			var bytes = new List<byte>();
			var part1 = BitConverter.GetBytes(0x56781234abcdef00);
			var part2 = BitConverter.GetBytes(0xbcdefabcd0011224);
			bytes.AddRange(part2);
			bytes.AddRange(part1);
			var big = new Org.BouncyCastle.Math.BigInteger(bytes.ToArray());
			return big;

		}
		public void Write1(BinaryWriter writer, byte[] auth, byte[] key, int size, byte[] clientOnce, ulong m_pq)
		{

			writer.Write(auth);
			writer.Write(key);
			writer.Write(size);
			var res = new MTResPQ();
			res.Nonce = new BigInteger(clientOnce);
			res.Pq = BitConverter.GetBytes(m_pq);
			var vector = new MTProto.NET.Schema.TL.TLVector<long>();
			var finger = RsaKey.GetFingerPrint();
			var finger2 = BitConverter.ToInt64(finger); ;
			vector.Add(finger2);
			res.ServerPublicKeyFingerprints = vector;

			var bytes = new List<byte>();
			var part1 = BitConverter.GetBytes(0x56781234abcdef00);
			var part2 = BitConverter.GetBytes(0x56781234abcdef00);
			bytes.AddRange(part2);
			bytes.AddRange(part1);

			var _ServerNonce = new Org.BouncyCastle.Math.BigInteger(BitConverter.GetBytes(0x56781234abcdef00));
			_ServerNonce.Multiply(new Org.BouncyCastle.Math.BigInteger(BitConverter.GetBytes(0xbcdefabcd0011224)));
			res.ServerNonce = ServerOnce();// new Org.BouncyCastle.Math.BigInteger(bytes.ToArray());
			MTObjectSerializer.Serialize(res, writer);
			//writer.Write('c');
			//writer.Close();

		}
		public void Write2(BinaryWriter writer, byte[] auth, byte[] key, int size, byte[] clientOnce, ulong m_pq)
		{
			writer.Write(auth);
			writer.Write(key);
			writer.Write(size);
			writer.Write(0x05162463);
			writer.Write(clientOnce);

			//writer.Write(0x56781234abcdef00);
			//writer.Write(0xbcdefabcd0011224);
			writer.Write(ServerOnce().ToByteArray());
			var __bytes = BitConverter.GetBytes(m_pq);
			writer.Write(BitConverter.GetBytes(m_pq));
			const int vectorConstructorNumber = 0x1cb5c415;
			writer.Write(vectorConstructorNumber);
			writer.Write(1);
			writer.Write(RsaKey.GetFingerPrint());

		}

		public int CompareBytes(byte[] b1, byte[] b2)
		{
			for (int i = 0; i < b1.Length; i++)
			{
				if (b1[i] != b2[i])
				{
					return i;
				}
			}
			return 0;
		}

		public async Task Handle(HttpContext context, Func<Task> next)
		{
			if (1 == 0)
			{
				await next();
				return;
			}

			if (1 == 0)
			{

				var stream = context.Request.Body;
				var reader = new BinaryReader(stream);
				//var auth = reader.ReadBytes(8);
				//var key = reader.ReadBytes(8);
				//var size = reader.ReadInt32();
				//var _o = MTObjectSerializer.Deserialize(reader);
				//var o = _o as MTReqPq;
				var writer = new BinaryWriter(context.Response.Body);
				var req = new RequestHandlingContext(reader, writer);
				//{
				//	AuthKey = auth,
				//	MessageId = key,
				//	Size = size,
				//	Output = writer

				//};
				await req.Handle();

				//var handler = new MTReqPqHandler();
				//handler.Handle(o, req);
				writer.Close();
			}
			if (1 == 1)
			{
				//var stream = context.Request.Body;
				//var reader = new BinaryReader(stream);
				//var writer = new BinaryWriter(context.Response.Body);
				//var auth = reader.ReadBytes(8);
				//var key = reader.ReadBytes(8);
				//var size = reader.ReadInt32();

				var _context = new MTProtoContext(context);
				await _context.Handle();
				//writer.Write(auth);
				//writer.Write(key);
				//writer.Write(size);
				//MTObjectSerializer.Serialize(_context.Response as MTObject, writer);


				if (1 == 0)
				{

					//var Obj = MTObjectSerializer.Deserialize(reader);
					//var res = await Handlers.Handlers.GetHandlerEx(Obj).Handle(Obj);
					//writer.Write(auth);
					//writer.Write(key);
					//writer.Write(size);
					//MTObjectSerializer.Serialize(res as MTObject, writer);
				}


				////var _o = MTObjectSerializer.Deserialize(reader);
				////var o = _o as MTReqPq;
				//var writer = new BinaryWriter(context.Response.Body);
				//var req = new RequestHandlingContext(reader, writer);
			}



			//var o = MTObjectSerializer.Deserialize(reader) as MTReqPq;

		}
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseCors("cors");
			app.UseRouting();
			var m_p = 1244159563ul;
			var m_q = 1558201013ul;
			var m_pq = m_p * m_q;
			app.Use(Handle);
			app.Use(async (context, next) =>
			{
				//next()
				//context.Response.ContentType = "application/octet-stream";
				var stream = context.Request.Body;
				var reader = new BinaryReader(stream);
				var auth = reader.ReadBytes(8);
				var key = reader.ReadBytes(8);
				var size = reader.ReadInt32();
				var writer = new BinaryWriter(context.Response.Body);
				var _memory = new MemoryStream();
				var writer2 = writer;// new BinaryWriter(_memory);
				var i = 1662825924;
				if (1 == 1)
				{
					//var o1 = MTObjectSerializer.Deserialize(reader) as MTReqPq;
					var o = MTObjectSerializer.Deserialize(reader) as MTReqPq;

					//var req = new RequestHandlingContext()
					//{
					//	AuthKey = auth,
					//	MessageId = key,
					//	Size = size,
					//	Output = writer
						
					//};

					//var handler = new MTReqPqHandler();
					//handler.Handle(o, req);
					//writer.Close();
					//return;
					



					writer2.Write(auth);
					writer2.Write(key);
					writer2.Write(size);
					var res = new MTResPQ();
					res.Nonce = o.Nonce;
					res.Pq = BitConverter.GetBytes(m_pq);
					var vector = new MTProto.NET.Schema.TL.TLVector<long>();
					var finger = RsaKey.GetFingerPrint2();
					var finger2 = BitConverter.ToInt64(finger); ;
					vector.Add(finger2);
					res.ServerPublicKeyFingerprints = vector;

					var _ServerNonce = new Org.BouncyCastle.Math.BigInteger(BitConverter.GetBytes(0x56781234abcdef00));
					_ServerNonce.Add(new Org.BouncyCastle.Math.BigInteger(BitConverter.GetBytes(0xbcdefabcd0011224)));
					res.ServerNonce = new Org.BouncyCastle.Math.BigInteger(BitConverter.GetBytes(0x56781234abcdef00));
					MTObjectSerializer.Serialize(res, writer2);
					writer2.Write('c');
					writer2.Close();
					var data = _memory.ToArray();
					context.Response.Body.Close();

					return;

				}




				var ul = reader.ReadUInt32();
				// 0xd712e4be ReqDHParams
				// 0x60469778 ReqPQ

				var ReqPQ = 0x60469778;
				var ReqDHParams = 0xd712e4be;
				if (ul == ReqPQ)
				{
					var buff = reader.ReadBytes(16);

					/// 
					var memory1 = new MemoryStream();
					var memory2 = new MemoryStream();
					var writer_1 = new BinaryWriter(memory1);
					var writer_2 = new BinaryWriter(memory2);
					Write1(writer_1, auth, key, size, buff, m_pq);
					Write2(writer_2, auth, key, size, buff, m_pq);
					writer_1.Close();
					writer_2.Close();
					var data1 = memory1.ToArray();
					var data2 = memory2.ToArray();
					CompareBytes(data2, data1);

					try
					{
						//memory2.CopyTo(context.Response.Body);
						context.Response.Body.Write(data1);
						context.Response.Body.Close();
					}
					catch (Exception err)
					{

					}
					return;



					var ii = 0x05162463;
					writer.Write(auth);
					writer.Write(key);
					writer.Write(size);
					writer.Write(0x05162463);
					writer.Write(buff);
					writer.Write(0x56781234abcdef00);
					writer.Write(0xbcdefabcd0011224);
					writer.Write(BitConverter.GetBytes(m_pq));
					const int vectorConstructorNumber = 0x1cb5c415;
					writer.Write(vectorConstructorNumber);
					writer.Write(1);

					writer.Write(RsaKey.GetFingerPrint());

					//context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:8000");
					//context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
					//context.Response.Headers.Add("Access-Control-Allow-Headers", "*");

					context.Response.Body.Close();
				}
				else if (ul == ReqDHParams)
				{
					var nonce = reader.ReadBytes(16);
					var server_nonce1 = reader.ReadUInt64();
					var server_nonce2 = reader.ReadUInt64();
					var ggg1 = RsaKey.read(reader);
					var ggg2 = RsaKey.read(reader);
					var finger = reader.ReadUInt64();

				}



			});


			//app.UseHttpsRedirection();

			//app.UseRouting();

			//app.UseAuthorization();

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllers();
			//});
		}
	}
}
