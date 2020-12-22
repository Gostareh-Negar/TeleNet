using MTProto.NET.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Serialization
{
	class MTSerializationService : IMTSerializationService
	{
		public object Deserialize(byte[] bytes)
		{
			using (var memory = new MemoryStream(bytes, false))


			using (var reader = new BinaryReader(memory))
			{
				return MTObjectSerializer.Deserialize(reader) ;
			}
		}

		public void Serialize(MTObject data, BinaryWriter writer)
		{
			MTProto.NET.Serializers.MTObjectSerializer.Serialize(data, writer);
		}
		public  byte[] Serialize(MTObject data)
		{
			using (var s = new MemoryStream())
			using (var w = new BinaryWriter(s))
			{
				Serialize(data, w);
				w.Close();
				return s.ToArray();
			}
		}

	}
}
