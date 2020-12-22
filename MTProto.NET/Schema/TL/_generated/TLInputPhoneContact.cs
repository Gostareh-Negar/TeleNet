using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xf392b7f4)]
    public class TLInputPhoneContact : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xf392b7f4;
            }
        }

        [MTParameter(Order = 0)]
        public long ClientId { get; set; }
        [MTParameter(Order = 1)]
        public string Phone { get; set; }
        [MTParameter(Order = 2)]
        public string FirstName { get; set; }
        [MTParameter(Order = 3)]
        public string LastName { get; set; }


    }
}
