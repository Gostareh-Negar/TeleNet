using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Account
{
    [MTObject(0x56da0b3f)]
    public class TLGetAutoDownloadSettings : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x56da0b3f;
            }
        }



    }
}
