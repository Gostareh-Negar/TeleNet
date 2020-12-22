using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL.Requests.Channels
{
    [MTObject(0xf8b036af)]
    public class TLGetAdminedPublicChannels : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xf8b036af;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
        public bool ByLocation { get; set; }
        [MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.True)]
        public bool CheckLimit { get; set; }


    }
}
