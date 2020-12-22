using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		public static bool IsMTPPacket(this HttpContext context)
		{
			return true;
		}
	}
}
