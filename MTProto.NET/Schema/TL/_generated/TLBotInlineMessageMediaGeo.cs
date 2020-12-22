using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xb722de65)]
    public class TLBotInlineMessageMediaGeo : TLAbsBotInlineMessage
    {
        public override uint Constructor
        {
            get
            {
                return 0xb722de65;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1)]
        public TLAbsGeoPoint Geo { get; set; }
        [MTParameter(Order = 2)]
        public int Period { get; set; }
        [MTParameter(Order = 3, FlagBitId = 2, FlagType = FlagType.Null)]
        public TLAbsReplyMarkup ReplyMarkup { get; set; }


    }
}
