using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Contacts
{
    [MTObject(0x7f077ad9)]
    public class TLResolvedPeer : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x7f077ad9;
            }
        }

        [MTParameter(Order = 0)]
        public TLAbsPeer Peer { get; set; }
        [MTParameter(Order = 1)]
        public TLVector<TLAbsChat> Chats { get; set; }
        [MTParameter(Order = 2)]
        public TLVector<TLAbsUser> Users { get; set; }


    }
}
