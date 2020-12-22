using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x6319d612)]
    public class TLDocumentAttributeSticker : TLAbsDocumentAttribute
    {
        public override uint Constructor
        {
            get
            {
                return 0x6319d612;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 1, FlagType = FlagType.True)]
        public bool Mask { get; set; }
        [MTParameter(Order = 2)]
        public string Alt { get; set; }
        [MTParameter(Order = 3)]
        public TLAbsInputStickerSet Stickerset { get; set; }
        [MTParameter(Order = 4, FlagBitId = 0, FlagType = FlagType.Null)]
        public MTProto.NET.Schema.TL.TLMaskCoords MaskCoords { get; set; }


    }
}
