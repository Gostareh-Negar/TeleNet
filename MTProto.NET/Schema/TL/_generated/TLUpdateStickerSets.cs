using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x43ae3dec)]
    public class TLUpdateStickerSets : TLAbsUpdate
    {
        public override uint Constructor
        {
            get
            {
                return 0x43ae3dec;
            }
        }



    }
}
