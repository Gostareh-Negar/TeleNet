using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xf888fa1a)]
    public class TLPrivacyValueDisallowContacts : TLAbsPrivacyRule
    {
        public override uint Constructor
        {
            get
            {
                return 0xf888fa1a;
            }
        }



    }
}
