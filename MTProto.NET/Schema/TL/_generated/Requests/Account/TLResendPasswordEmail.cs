using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Account
{
    [MTObject(0x7a7f2a15)]
    public class TLResendPasswordEmail : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x7a7f2a15;
            }
        }



    }
}
