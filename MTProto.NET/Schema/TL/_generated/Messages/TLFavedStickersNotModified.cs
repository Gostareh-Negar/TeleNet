using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Messages
{
    [MTObject(0x9e8fa6d3)]
    public class TLFavedStickersNotModified : TLAbsFavedStickers
    {
        public override uint Constructor
        {
            get
            {
                return 0x9e8fa6d3;
            }
        }



    }
}
