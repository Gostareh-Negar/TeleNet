using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x9f84f49e)]
    public class TLMessageMediaUnsupported : TLAbsMessageMedia
    {
        public override uint Constructor
        {
            get
            {
                return 0x9f84f49e;
            }
        }



    }
}
