using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Channels
{
    [MTObject(0xed8af74d)]
    public class TLAdminLogResults : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xed8af74d;
            }
        }

        [MTParameter(Order = 0)]
        public TLVector<MTProto.NET.Schema.TL.TLChannelAdminLogEvent> Events { get; set; }
        [MTParameter(Order = 1)]
        public TLVector<TLAbsChat> Chats { get; set; }
        [MTParameter(Order = 2)]
        public TLVector<TLAbsUser> Users { get; set; }


    }
}
