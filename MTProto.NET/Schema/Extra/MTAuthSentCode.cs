using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.Extra
{
	//auth.sentCode#5e002502 flags:# phone_registered:flags.0?true type:auth.SentCodeType phone_code_hash:string next_type:flags.1?auth.CodeType timeout:flags.2?int = auth.SentCode;

	[MTObject(0x5e002502)]
	public class MTAuthSentCode :MTObject
	{
		public override uint Constructor
		{
			get
			{
				return 0x5e002502;
			}
		}

		[MTParameter(Order = 0, IsFlag = true)]
		public int Flags { get; set; }

		[MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
		public bool PhoneRegistered { get; set; }

		[MTParameter(Order = 2)]
		public bool SentCodeType { get; set; }

		[MTParameter(Order = 3)]
		public string PhoneCodeHash { get; set; }

		[MTParameter(Order = 4, FlagBitId = 1, FlagType = FlagType.Null)]
		public int CodeType { get; set; }

		[MTParameter(Order = 5, FlagBitId = 1, FlagType = FlagType.Null)]
		public int TimeOut { get; set; }
	}
}
