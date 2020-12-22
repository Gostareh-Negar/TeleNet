using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;
using MTProto.NET.Schema.TL;

namespace MTProto.NET.Schema.Layer72
{
	/* chat#d91cdd54 
     * flags:# 
     * creator:flags.0?true 
     * kicked:flags.1?true 
	 * left:flags.2?true 
	 * admins_enabled:flags.3?true 
	 * admin:flags.4?true 
	 * deactivated:flags.5?true 
	 * id:int 
	 * title:string 
	 * photo:ChatPhoto 
	 * participants_count:int 
	 * date:int 
	 * version:int 
	 * migrated_to:flags.6?InputChannel = Chat;
	 * 
	 * */
	[MTObject(0xd91cdd54)]
	public class TLChat : TLAbsChat
	{
		public override uint Constructor
		{
			get
			{
				return 0xd91cdd54;
			}
		}

		[MTParameter(Order = 0, IsFlag = true)]
		public int Flags { get; set; }
		[MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
		public bool Creator { get; set; }
		[MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.True)]
		public bool Kicked { get; set; }
		[MTParameter(Order = 3, FlagBitId = 2, FlagType = FlagType.True)]
		public bool Left { get; set; }

		//[MTParameter(Order = 4, FlagBitId = 5, FlagType = FlagType.True)]
		//public bool Deactivated { get; set; }
		[MTParameter(Order = 4, FlagBitId = 3, FlagType = FlagType.True)]
		public bool admins_enabled { get; set; }

		[MTParameter(Order = 5, FlagBitId = 4, FlagType = FlagType.True)]
		public bool admin { get; set; }



		[MTParameter(Order = 6)]
		public int Id { get; set; }
		[MTParameter(Order = 7)]
		public string Title { get; set; }
		[MTParameter(Order = 8)]
		public TLAbsChatPhoto Photo { get; set; }
		[MTParameter(Order = 9)]
		public int ParticipantsCount { get; set; }
		[MTParameter(Order = 10)]
		public int Date { get; set; }
		[MTParameter(Order = 10)]
		public int Version { get; set; }
		[MTParameter(Order = 11, FlagBitId = 6, FlagType = FlagType.Null)]
		public TLAbsInputChannel MigratedTo { get; set; }
		//[MTParameter(Order = 11, FlagBitId = 14, FlagType = FlagType.Null)]
		//public MTProto.NET.Schema.TL.TLChatAdminRights AdminRights { get; set; }
		//[MTParameter(Order = 13, FlagBitId = 18, FlagType = FlagType.Null)]
		//public MTProto.NET.Schema.TL.TLChatBannedRights DefaultBannedRights { get; set; }
	}
	
}

