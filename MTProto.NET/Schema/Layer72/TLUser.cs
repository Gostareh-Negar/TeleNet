using MTProto.NET.Schema.TL;
using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.Layer72
{
    // user#2e13f4c3 
    //  flags:# 
    //  self:flags.10?true 
    //  contact:flags.11?true 
    //  mutual_contact:flags.12?true 
    //  deleted:flags.13?true 
    //  bot:flags.14?true 
    //  bot_chat_history:flags.15?true 
    //  bot_nochats:flags.16?true verified:flags.17?true 
    //  restricted:flags.18?true 
    //  min:flags.20?true 
    //  bot_inline_geo:flags.21?true 
    //  id:int access_hash:flags.0?long 
    //  first_name:flags.1?string 
    //  last_name:flags.2?string 
    //  username:flags.3?string 
    //  phone:flags.4?string 
    //  photo:flags.5?UserProfilePhoto 
    //  status:flags.6?UserStatus 
    //  bot_info_version:flags.14?int 
    //  restriction_reason:flags.18?string 
    //  bot_inline_placeholder:flags.19?
    //  string lang_code:flags.22?string = User;

    [MTObject(0x2e13f4c3)]
    public class TLUser : TLAbsUser
    {
        public override uint Constructor
        {
            get
            {
                return 0x2e13f4c3;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 10, FlagType = FlagType.True)]
        public bool Self { get; set; }
        [MTParameter(Order = 2, FlagBitId = 11, FlagType = FlagType.True)]
        public bool Contact { get; set; }
        [MTParameter(Order = 3, FlagBitId = 12, FlagType = FlagType.True)]
        public bool MutualContact { get; set; }
        [MTParameter(Order = 4, FlagBitId = 13, FlagType = FlagType.True)]
        public bool Deleted { get; set; }
        [MTParameter(Order = 5, FlagBitId = 14, FlagType = FlagType.True)]
        public bool Bot { get; set; }
        [MTParameter(Order = 6, FlagBitId = 15, FlagType = FlagType.True)]
        public bool BotChatHistory { get; set; }
        [MTParameter(Order = 7, FlagBitId = 16, FlagType = FlagType.True)]
        public bool BotNochats { get; set; }
        [MTParameter(Order = 8, FlagBitId = 17, FlagType = FlagType.True)]
        public bool Verified { get; set; }
        [MTParameter(Order = 9, FlagBitId = 18, FlagType = FlagType.True)]
        public bool Restricted { get; set; }
        [MTParameter(Order = 10, FlagBitId = 20, FlagType = FlagType.True)]
        public bool Min { get; set; }
        [MTParameter(Order = 11, FlagBitId = 21, FlagType = FlagType.True)]
        public bool BotInlineGeo { get; set; }
        [MTParameter(Order = 12, FlagBitId = 23, FlagType = FlagType.True)]
        public bool Support { get; set; }
        [MTParameter(Order = 13, FlagBitId = 24, FlagType = FlagType.True)]
        public bool Scam { get; set; }
        [MTParameter(Order = 14, FlagBitId = 25, FlagType = FlagType.True)]
        public bool ApplyMinPhoto { get; set; }
        [MTParameter(Order = 15)]
        public int Id { get; set; }
        [MTParameter(Order = 16, FlagBitId = 0, FlagType = FlagType.Null)]
        public long? AccessHash { get; set; }
        [MTParameter(Order = 17, FlagBitId = 1, FlagType = FlagType.Null)]
        public string FirstName { get; set; }
        [MTParameter(Order = 18, FlagBitId = 2, FlagType = FlagType.Null)]
        public string LastName { get; set; }
        [MTParameter(Order = 19, FlagBitId = 3, FlagType = FlagType.Null)]
        public string Username { get; set; }
        [MTParameter(Order = 20, FlagBitId = 4, FlagType = FlagType.Null)]
        public string Phone { get; set; }
        [MTParameter(Order = 21, FlagBitId = 5, FlagType = FlagType.Null)]
        public TLAbsUserProfilePhoto Photo { get; set; }
        [MTParameter(Order = 22, FlagBitId = 6, FlagType = FlagType.Null)]
        public TLAbsUserStatus Status { get; set; }
        [MTParameter(Order = 23, FlagBitId = 14, FlagType = FlagType.Null)]
        public int? BotInfoVersion { get; set; }
        [MTParameter(Order = 24, FlagBitId = 18, FlagType = FlagType.Null)]
        public TLVector<MTProto.NET.Schema.TL.TLRestrictionReason> RestrictionReason { get; set; }
        [MTParameter(Order = 25, FlagBitId = 19, FlagType = FlagType.Null)]
        public string BotInlinePlaceholder { get; set; }
        [MTParameter(Order = 26, FlagBitId = 22, FlagType = FlagType.Null)]
        public string LangCode { get; set; }


    }
}
