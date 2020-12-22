using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Contacts
{
    [MTObject(0xb3134d9d)]
    public class TLFound : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xb3134d9d;
            }
        }

        [MTParameter(Order = 0)]
        public TLVector<TLAbsPeer> MyResults { get; set; }
        [MTParameter(Order = 1)]
        public TLVector<TLAbsPeer> Results { get; set; }
        [MTParameter(Order = 2)]
        public TLVector<TLAbsChat> Chats { get; set; }
        [MTParameter(Order = 3)]
        public TLVector<TLAbsUser> Users { get; set; }


    }
}
