using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xf4e096c3)]
    public class TLInputMediaInvoice : TLAbsInputMedia
    {
        public override uint Constructor
        {
            get
            {
                return 0xf4e096c3;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1)]
        public string Title { get; set; }
        [MTParameter(Order = 2)]
        public string Description { get; set; }
        [MTParameter(Order = 3, FlagBitId = 0, FlagType = FlagType.Null)]
        public MTProto.NET.Schema.TL.TLInputWebDocument Photo { get; set; }
        [MTParameter(Order = 4)]
        public MTProto.NET.Schema.TL.TLInvoice Invoice { get; set; }
        [MTParameter(Order = 5)]
        public byte[] Payload { get; set; }
        [MTParameter(Order = 6)]
        public string Provider { get; set; }
        [MTParameter(Order = 7)]
        public MTProto.NET.Schema.TL.TLDataJSON ProviderData { get; set; }
        [MTParameter(Order = 8)]
        public string StartParam { get; set; }


    }
}
