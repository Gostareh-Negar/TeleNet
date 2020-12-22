using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Helpers
{
	public static class DateTimeHelper
	{
		public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static int ToTelegarmDate(DateTime utcTime)
		{
			return Convert.ToInt32((DateTime.UtcNow - Epoch).TotalSeconds);
		}
		public static ulong CurrentMsecsFromEpoch()
		{
			
			return (ulong)(DateTime.UtcNow - Epoch).TotalMilliseconds;
		}
		public static ulong FormatTimeSTamp(ulong timeInMs)
		{
			ulong max = ((ulong)1 << 32) - 1;
			ulong seconds = timeInMs / 1000ul;
			ulong msecs = max / 1000ul + (timeInMs % 1000ul);
			return (seconds << 32) + msecs;
		}
		public static ulong TimeStampToMSescsSinceEpoch(ulong ts)
		{
			ulong max = ((ulong)1 << 32) - 1;
			ulong secs = ts >> 32;
			ulong msecs = ts & max;
			msecs = msecs * 1000ul / max;
			if (msecs % 10 >= 5)
			{
				msecs += 5;
			}
			msecs /= 10;
			return secs * 1000 + msecs;

		}

	}
}
