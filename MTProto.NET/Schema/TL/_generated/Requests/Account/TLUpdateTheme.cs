using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL.Requests.Account
{
    [MTObject(0x5cb367d5)]
    public class TLUpdateTheme : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x5cb367d5;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1)]
        public string Format { get; set; }
        [MTParameter(Order = 2)]
        public TLAbsInputTheme Theme { get; set; }
        [MTParameter(Order = 3, FlagBitId = 0, FlagType = FlagType.Null)]
        public string Slug { get; set; }
        [MTParameter(Order = 4, FlagBitId = 1, FlagType = FlagType.Null)]
        public string Title { get; set; }
        [MTParameter(Order = 5, FlagBitId = 2, FlagType = FlagType.Null)]
        public TLAbsInputDocument Document { get; set; }
        [MTParameter(Order = 6, FlagBitId = 3, FlagType = FlagType.Null)]
        public MTProto.NET.Schema.TL.TLInputThemeSettings Settings { get; set; }


    }
}
