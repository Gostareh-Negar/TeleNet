using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL.Requests.Messages
{
    [MTObject(0xf729ea98)]
    public class TLAcceptUrlAuth : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xf729ea98;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
        public bool WriteAllowed { get; set; }
        [MTParameter(Order = 2)]
        public TLAbsInputPeer Peer { get; set; }
        [MTParameter(Order = 3)]
        public int MsgId { get; set; }
        [MTParameter(Order = 4)]
        public int ButtonId { get; set; }


    }
}
