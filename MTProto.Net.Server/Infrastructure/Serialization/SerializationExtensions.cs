using Microsoft.Extensions.DependencyInjection;
using MTProto.NET.Serializers;
using MTProto.NET.Server.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		public static MTObject ReadMTObject(this BinaryReader reader)
		{
			return MTObjectSerializer.Deserialize(reader);
		}
		internal static IServiceCollection AddSerialization( this IServiceCollection services)
		{
			services.AddTransient<IMTSerializationService, MTSerializationService>();
			return services;
		}
		public static IMTSerializationService Serialization(this IMTServiceProvider provider)
		{
			return provider.GetService<IMTSerializationService>();

		}
		public static byte[] ToByteArray (this MTObject data)
		{
			return MTServer.Services.Serialization().Serialize(data);

		}
		public static MTObject ToMTObject(this byte[] bytes)
		{
			using(var mem = new MemoryStream(bytes))
				using(var reader = new BinaryReader(mem))
			{
				return MTObjectSerializer.Deserialize(reader);
			}
		}
	}
}
