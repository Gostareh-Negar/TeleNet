using MTProto.NET.Schema.TL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Serializers
{
	public class VectorSerializer<T> : ISerializer<TLVector<T>>
	{
		public static TLVector<T> Deserialize(BinaryReader reader)
		{
			TLVector<T> vector = new TLVector<T>();

			// Babak 
			//uint constructor = reader.ReadUInt32();
			//if (constructor != vector.Constructor) throw new InvalidOperationException("Vector deserialization failed, this is not a vector!");

			//int count = reader.ReadInt32();
			uint count_or_constructor = reader.ReadUInt32();
			uint count = 0;
			if (count_or_constructor < 1000)
			{
				count = count_or_constructor;
			}
			else if (count_or_constructor != vector.Constructor)
			{
				throw new InvalidOperationException("Vector deserialization failed, this is not a vector!");
			}
			else
			{
				count = reader.ReadUInt32();
			}

			for (int i = 0; i < count; i++)
			{
				if (typeof(MTObject).IsAssignableFrom(typeof(T)))
				{
					vector.Add((T) (object) MTObjectSerializer.Deserialize(reader));
				}
				else
				{
					vector.Add((T)SerializationContext.Deserialize(reader, typeof(T)));
				}
			}

			return vector;
		}

		public static void Serialize(TLVector<T> vector, BinaryWriter writer)
		{
			writer.Write(vector.Constructor);
			writer.Write(vector.Count);

			foreach (var item in vector)
			{
				SerializationContext.Serialize(writer, item);
			}
		}
	}
}
