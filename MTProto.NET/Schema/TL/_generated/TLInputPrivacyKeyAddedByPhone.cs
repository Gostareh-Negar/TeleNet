using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xd1219bdd)]
    public class TLInputPrivacyKeyAddedByPhone : TLAbsInputPrivacyKey
    {
        public override uint Constructor
        {
            get
            {
                return 0xd1219bdd;
            }
        }



    }
}
