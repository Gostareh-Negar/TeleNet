using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xb320aadb)]
    public class TLSecureValueTypePhone : TLAbsSecureValueType
    {
        public override uint Constructor
        {
            get
            {
                return 0xb320aadb;
            }
        }



    }
}
