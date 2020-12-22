using MTProto.NET;
using MTProto.NET.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Serialization
{
	public class MTStreamReader : BinaryReader, IMTStreamReader
	{

		public MTStreamReader(Stream stream) : base(stream)
		{

		}
		public MTObject ReadObject()
		{
			return MTObjectSerializer.Deserialize(this);
		}
		public IMTStreamReader Split()
		{
			var stream = new MemoryStream();
			this.BaseStream.CopyTo(stream);
			stream.Seek(0, SeekOrigin.Begin);
			return new MTStreamReader(stream);
		}
		public byte[] ToBytes()
		{
			this.BaseStream.Seek(0, SeekOrigin.Begin);
			return (this.BaseStream as MemoryStream).ToArray();
			
		}
	}
}
