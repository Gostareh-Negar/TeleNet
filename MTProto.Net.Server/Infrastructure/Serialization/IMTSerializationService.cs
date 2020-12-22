using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Serialization
{
	public interface IMTSerializationService
	{
		byte[] Serialize(MTObject data);
		void Serialize(MTObject data, BinaryWriter writer);
		object Deserialize(byte[] bytes);
	}
}
