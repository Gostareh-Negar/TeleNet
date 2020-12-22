using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xdfdaabe1)]
    public class TLInputFileLocation : TLAbsInputFileLocation
    {
        public override uint Constructor
        {
            get
            {
                return 0xdfdaabe1;
            }
        }

        [MTParameter(Order = 0)]
        public long VolumeId { get; set; }
        [MTParameter(Order = 1)]
        public int LocalId { get; set; }
        [MTParameter(Order = 2)]
        public long Secret { get; set; }
        [MTParameter(Order = 3)]
        public byte[] FileReference { get; set; }


    }
}
