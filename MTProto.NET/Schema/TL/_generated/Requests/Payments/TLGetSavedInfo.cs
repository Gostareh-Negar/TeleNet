using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Payments
{
    [MTObject(0x227d824b)]
    public class TLGetSavedInfo : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x227d824b;
            }
        }



    }
}
