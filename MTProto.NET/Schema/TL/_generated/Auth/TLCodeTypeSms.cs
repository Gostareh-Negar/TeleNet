using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Auth
{
    [MTObject(0x72a3158c)]
    public class TLCodeTypeSms : TLAbsCodeType
    {
        public override uint Constructor
        {
            get
            {
                return 0x72a3158c;
            }
        }



    }
}
