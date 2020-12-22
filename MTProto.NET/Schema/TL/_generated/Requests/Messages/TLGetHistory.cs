using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Messages
{
    /*
     * References:
     *  1. https://core.telegram.org/method/messages.getHistory
     */
    [MTObject(0xdcbb8260)]
    public class TLGetHistory : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xdcbb8260;
            }
        }

        [MTParameter(Order = 0)]
        public TLAbsInputPeer Peer { get; set; }
        [MTParameter(Order = 1)]
        public int OffsetId { get; set; }
        [MTParameter(Order = 2)]
        public int OffsetDate { get; set; }
        [MTParameter(Order = 3)]
        public int AddOffset { get; set; }
        [MTParameter(Order = 4)]
        public int Limit { get; set; }
        [MTParameter(Order = 5)]
        public int MaxId { get; set; }
        [MTParameter(Order = 6)]
        public int MinId { get; set; }
        [MTParameter(Order = 7)]
        public int Hash { get; set; }


    }
}
