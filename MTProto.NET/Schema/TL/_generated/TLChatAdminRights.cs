using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x5fb224d5)]
    public class TLChatAdminRights : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x5fb224d5;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
        public bool ChangeInfo { get; set; }
        [MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.True)]
        public bool PostMessages { get; set; }
        [MTParameter(Order = 3, FlagBitId = 2, FlagType = FlagType.True)]
        public bool EditMessages { get; set; }
        [MTParameter(Order = 4, FlagBitId = 3, FlagType = FlagType.True)]
        public bool DeleteMessages { get; set; }
        [MTParameter(Order = 5, FlagBitId = 4, FlagType = FlagType.True)]
        public bool BanUsers { get; set; }
        [MTParameter(Order = 6, FlagBitId = 5, FlagType = FlagType.True)]
        public bool InviteUsers { get; set; }
        [MTParameter(Order = 7, FlagBitId = 7, FlagType = FlagType.True)]
        public bool PinMessages { get; set; }
        [MTParameter(Order = 8, FlagBitId = 9, FlagType = FlagType.True)]
        public bool AddAdmins { get; set; }


    }
}
