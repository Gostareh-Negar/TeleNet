using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x9b9240a6)]
    public class TLUpdateBotWebhookJSONQuery : TLAbsUpdate
    {
        public override uint Constructor
        {
            get
            {
                return 0x9b9240a6;
            }
        }

        [MTParameter(Order = 0)]
        public long QueryId { get; set; }
        [MTParameter(Order = 1)]
        public MTProto.NET.Schema.TL.TLDataJSON Data { get; set; }
        [MTParameter(Order = 2)]
        public int Timeout { get; set; }


    }
}
