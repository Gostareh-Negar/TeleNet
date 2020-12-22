using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x1c0facaf)]
    public class TLChannelParticipantBanned : TLAbsChannelParticipant
    {
        public override uint Constructor
        {
            get
            {
                return 0x1c0facaf;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
        public bool Left { get; set; }
        [MTParameter(Order = 2)]
        public int UserId { get; set; }
        [MTParameter(Order = 3)]
        public int KickedBy { get; set; }
        [MTParameter(Order = 4)]
        public int Date { get; set; }
        [MTParameter(Order = 5)]
        public MTProto.NET.Schema.TL.TLChatBannedRights BannedRights { get; set; }


    }
}
