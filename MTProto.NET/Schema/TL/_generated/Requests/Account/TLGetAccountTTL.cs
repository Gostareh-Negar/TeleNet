using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Account
{
    [MTObject(0x8fc711d)]
    public class TLGetAccountTTL : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x8fc711d;
            }
        }



    }
}
