using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Updates
{
    [MTObject(0xedd4882a)]
    public class TLGetState : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xedd4882a;
            }
        }



    }
}
