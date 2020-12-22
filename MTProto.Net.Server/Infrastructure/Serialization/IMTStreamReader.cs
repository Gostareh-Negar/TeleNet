using MTProto.NET;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Serialization
{
	public interface IMTStreamReader : IDisposable
	{
		MTObject ReadObject();
		IMTStreamReader Split();
		byte[] ToBytes();
	}
}
