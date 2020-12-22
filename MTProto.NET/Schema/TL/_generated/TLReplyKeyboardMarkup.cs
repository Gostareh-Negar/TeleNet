using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x3502758c)]
    public class TLReplyKeyboardMarkup : TLAbsReplyMarkup
    {
        public override uint Constructor
        {
            get
            {
                return 0x3502758c;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
        public bool Resize { get; set; }
        [MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.True)]
        public bool SingleUse { get; set; }
        [MTParameter(Order = 3, FlagBitId = 2, FlagType = FlagType.True)]
        public bool Selective { get; set; }
        [MTParameter(Order = 4)]
        public TLVector<MTProto.NET.Schema.TL.TLKeyboardButtonRow> Rows { get; set; }


    }
}
