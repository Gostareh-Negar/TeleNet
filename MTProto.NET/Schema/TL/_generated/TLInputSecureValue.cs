using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xdb21d0a7)]
    public class TLInputSecureValue : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xdb21d0a7;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1)]
        public TLAbsSecureValueType Type { get; set; }
        [MTParameter(Order = 2, FlagBitId = 0, FlagType = FlagType.Null)]
        public MTProto.NET.Schema.TL.TLSecureData Data { get; set; }
        [MTParameter(Order = 3, FlagBitId = 1, FlagType = FlagType.Null)]
        public TLAbsInputSecureFile FrontSide { get; set; }
        [MTParameter(Order = 4, FlagBitId = 2, FlagType = FlagType.Null)]
        public TLAbsInputSecureFile ReverseSide { get; set; }
        [MTParameter(Order = 5, FlagBitId = 3, FlagType = FlagType.Null)]
        public TLAbsInputSecureFile Selfie { get; set; }
        [MTParameter(Order = 6, FlagBitId = 6, FlagType = FlagType.Null)]
        public TLVector<TLAbsInputSecureFile> Translation { get; set; }
        [MTParameter(Order = 7, FlagBitId = 4, FlagType = FlagType.Null)]
        public TLVector<TLAbsInputSecureFile> Files { get; set; }
        [MTParameter(Order = 8, FlagBitId = 5, FlagType = FlagType.Null)]
        public TLAbsSecurePlainData PlainData { get; set; }


    }
}
