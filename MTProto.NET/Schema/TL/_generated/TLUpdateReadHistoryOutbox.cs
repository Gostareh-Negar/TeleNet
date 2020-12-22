using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x2f2f21bf)]
    public class TLUpdateReadHistoryOutbox : TLAbsUpdate
    {
        public override uint Constructor
        {
            get
            {
                return 0x2f2f21bf;
            }
        }

        [MTParameter(Order = 0)]
        public TLAbsPeer Peer { get; set; }
        [MTParameter(Order = 1)]
        public int MaxId { get; set; }
        [MTParameter(Order = 2)]
        public int Pts { get; set; }
        [MTParameter(Order = 3)]
        public int PtsCount { get; set; }


    }
}
