using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x6410a5d2)]
    public class TLStickerSetCovered : TLAbsStickerSetCovered
    {
        public override uint Constructor
        {
            get
            {
                return 0x6410a5d2;
            }
        }

        [MTParameter(Order = 0)]
        public MTProto.NET.Schema.TL.TLStickerSet Set { get; set; }
        [MTParameter(Order = 1)]
        public TLAbsDocument Cover { get; set; }


    }
}
