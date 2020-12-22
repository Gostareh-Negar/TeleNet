using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x3407e51b)]
    public class TLStickerSetMultiCovered : TLAbsStickerSetCovered
    {
        public override uint Constructor
        {
            get
            {
                return 0x3407e51b;
            }
        }

        [MTParameter(Order = 0)]
        public MTProto.NET.Schema.TL.TLStickerSet Set { get; set; }
        [MTParameter(Order = 1)]
        public TLVector<TLAbsDocument> Covers { get; set; }


    }
}
