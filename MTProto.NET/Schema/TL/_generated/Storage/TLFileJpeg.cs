using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Storage
{
    [MTObject(0x7efe0e)]
    public class TLFileJpeg : TLAbsFileType
    {
        public override uint Constructor
        {
            get
            {
                return 0x7efe0e;
            }
        }



    }
}
