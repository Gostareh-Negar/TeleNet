using MTProto.NET.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Serialization
{
	public class MTStreamWriter : BinaryWriter
	{
		
		public MTStreamWriter() : base(new MemoryStream())
		{

		}
		public void Write(MTObject @object)
		{
			MTObjectSerializer.Serialize(@object, this);
		}
		public byte[] ToArray()
		{

			this.BaseStream.Close();
			return (this.BaseStream as MemoryStream).ToArray();
		}


	}
}
