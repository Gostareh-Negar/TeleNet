using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.Extra
{
	//
	// auth.sendCode#86aef0ec flags:# allow_flashcall:flags.0?true phone_number:string current_number:flags.0?Bool api_id:int api_hash:string = auth.SentCode
	// https://github.com/wfjsw/telegram-core-docs/blob/master/method/auth.sendCode.md
	[MTObject(0x86aef0ec)]
	public class MTAuthSendCode : MTObject
	{
		public override uint Constructor
		{
			get
			{
				return 0x86aef0ec;
			}
		}
		[MTParameter(Order = 0, IsFlag = true)]
		public int Flags { get; set; }

		[MTParameter(Order = 1)]
		public string Phone { get; set; }

		[MTParameter(Order = 2, FlagBitId = 0, FlagType = FlagType.True)]
		public bool CurrentNumber { get; set; }

		[MTParameter(Order = 3)]
		public int ApiId { get; set; }

		[MTParameter(Order = 4)]
		public string ApHash { get; set; }


	}
}
