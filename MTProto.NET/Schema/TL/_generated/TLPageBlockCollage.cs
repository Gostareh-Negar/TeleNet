using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x65a0fa4d)]
    public class TLPageBlockCollage : TLAbsPageBlock
    {
        public override uint Constructor
        {
            get
            {
                return 0x65a0fa4d;
            }
        }

        [MTParameter(Order = 0)]
        public TLVector<TLAbsPageBlock> Items { get; set; }
        [MTParameter(Order = 1)]
        public MTProto.NET.Schema.TL.TLPageCaption Caption { get; set; }


    }
}
