using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Messages
{
    [MTObject(0xe86602c3)]
    public class TLAllStickersNotModified : TLAbsAllStickers
    {
        public override uint Constructor
        {
            get
            {
                return 0xe86602c3;
            }
        }



    }
}
