using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Contacts
{
    [MTObject(0x70b772a8)]
    public class TLTopPeers : TLAbsTopPeers
    {
        public override uint Constructor
        {
            get
            {
                return 0x70b772a8;
            }
        }

        [MTParameter(Order = 0)]
        public TLVector<MTProto.NET.Schema.TL.TLTopPeerCategoryPeers> Categories { get; set; }
        [MTParameter(Order = 1)]
        public TLVector<TLAbsChat> Chats { get; set; }
        [MTParameter(Order = 2)]
        public TLVector<TLAbsUser> Users { get; set; }


    }
}
