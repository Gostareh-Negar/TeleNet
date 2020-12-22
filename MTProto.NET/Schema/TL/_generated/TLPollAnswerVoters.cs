using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x3b6ddad2)]
    public class TLPollAnswerVoters : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x3b6ddad2;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
        public bool Chosen { get; set; }
        [MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.True)]
        public bool Correct { get; set; }
        [MTParameter(Order = 3)]
        public byte[] Option { get; set; }
        [MTParameter(Order = 4)]
        public int Voters { get; set; }


    }
}
