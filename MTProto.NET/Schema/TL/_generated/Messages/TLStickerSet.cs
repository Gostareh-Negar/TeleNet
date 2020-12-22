using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Messages
{
    [MTObject(0xb60a24a6)]
    public class TLStickerSet : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xb60a24a6;
            }
        }

        [MTParameter(Order = 0)]
        public MTProto.NET.Schema.TL.TLStickerSet Set { get; set; }
        [MTParameter(Order = 1)]
        public TLVector<MTProto.NET.Schema.TL.TLStickerPack> Packs { get; set; }
        [MTParameter(Order = 2)]
        public TLVector<TLAbsDocument> Documents { get; set; }


    }
}
