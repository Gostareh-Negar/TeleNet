using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Helpers
{
	public static class Utils
	{
		public static byte[] compress(byte[] data)
		{
			using (var ms = new MemoryStream())
			{
				using (var gzip = new GZipStream(ms, CompressionLevel.Optimal))
				{
					gzip.Write(data, 0, data.Length);
				}
				data = ms.ToArray();
			}
			return data;
		}
		public static ulong CreateNewMessageId(ulong lastMessageId, bool isReply)
		{
			ulong result = DateTimeHelper.FormatTimeSTamp(DateTimeHelper.CurrentMsecsFromEpoch());
			if (isReply)
			{
				result &= ~3ul;
				result |= 1;
			}
			else
			{
				result |= 3;
			}
			ulong moduleFour = result % 4ul;

			if (result < lastMessageId)
			{
				result = (lastMessageId & ~3ul) | moduleFour;
				if (result <= lastMessageId)
					result += 4;
			}
			return result;
		}
		public static uint GetNewMessageSequenceNumber(uint contentRelatedMessageCout , bool isContentRelated)
		{
			var result = (contentRelatedMessageCout * 2u)  + (isContentRelated ? 1u : 0u);
			return result;
			
		}
	}
}
