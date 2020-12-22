using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x352dca58)]
    public class TLMessageEntityMentionName : TLAbsMessageEntity
    {
        public override uint Constructor
        {
            get
            {
                return 0x352dca58;
            }
        }

        [MTParameter(Order = 0)]
        public int Offset { get; set; }
        [MTParameter(Order = 1)]
        public int Length { get; set; }
        [MTParameter(Order = 2)]
        public int UserId { get; set; }


    }
}
