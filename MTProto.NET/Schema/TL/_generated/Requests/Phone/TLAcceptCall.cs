using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Phone
{
    [MTObject(0x3bd2b4a0)]
    public class TLAcceptCall : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x3bd2b4a0;
            }
        }

        [MTParameter(Order = 0)]
        public MTProto.NET.Schema.TL.TLInputPhoneCall Peer { get; set; }
        [MTParameter(Order = 1)]
        public byte[] GB { get; set; }
        [MTParameter(Order = 2)]
        public MTProto.NET.Schema.TL.TLPhoneCallProtocol Protocol { get; set; }


    }
}
