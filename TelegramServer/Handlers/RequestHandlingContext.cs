using MTProto.NET;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramServer.Handlers
{
	public interface IRequestHandlingContext
	{
		public byte[] AuthKey { get; set; }
		public byte[] MessageId { get; set; }

		public int Size { get; set; }

		BinaryWriter Output { get; set; }



	}
	public interface IRequestHandlingContext<TRequest> : IRequestHandlingContext
	{
		TRequest Request { get; set; }
	}
	public class RequestHandlingContext : IRequestHandlingContext
	{
		public byte[] AuthKey { get; set; }
		public byte[] MessageId { get; set; }
		public int Size { get; set; }
		public BinaryWriter Output { get; set; }
		public object Obj;

		public RequestHandlingContext(BinaryReader reader, BinaryWriter writer)
		{
			this.AuthKey = reader.ReadBytes(8);
			this.MessageId = reader.ReadBytes(8);
			this.Size = reader.ReadInt32();
			this.Output = writer;
			this.Obj = MTObjectSerializer.Deserialize(reader);

		}
		
		public async Task Handle()
		{
			object res = null;
			switch (this.Obj)
			{
				case MTReqPq hh:
					var handler = new MTReqPqHandler();
					//res = await handler.HandleEx(hh);
					break;
				case MTReqDhParams hh:
					{

					}
					break;

				default:
					break;
			}
			if (res as MTObject != null)
			{
				this.Output.Write(this.AuthKey);
				this.Output.Write(this.MessageId);
				this.Output.Write(this.Size);
				MTObjectSerializer.Serialize(res as MTObject, this.Output);
			}


		}
	}

	
}
