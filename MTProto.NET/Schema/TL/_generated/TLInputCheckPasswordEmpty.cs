using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x9880f658)]
    public class TLInputCheckPasswordEmpty : TLAbsInputCheckPasswordSRP
    {
        public override uint Constructor
        {
            get
            {
                return 0x9880f658;
            }
        }



    }
}
