using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x69df3769)]
    public class TLChatInviteEmpty : TLAbsExportedChatInvite
    {
        public override uint Constructor
        {
            get
            {
                return 0x69df3769;
            }
        }



    }
}
