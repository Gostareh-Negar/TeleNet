using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Phone
{
    [MTObject(0x2efe1722)]
    public class TLConfirmCall : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x2efe1722;
            }
        }

        [MTParameter(Order = 0)]
        public MTProto.NET.Schema.TL.TLInputPhoneCall Peer { get; set; }
        [MTParameter(Order = 1)]
        public byte[] GA { get; set; }
        [MTParameter(Order = 2)]
        public long KeyFingerprint { get; set; }
        [MTParameter(Order = 3)]
        public MTProto.NET.Schema.TL.TLPhoneCallProtocol Protocol { get; set; }


    }
}
