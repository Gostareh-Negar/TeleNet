using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0xf89777f2)]
    public class TLChannelAdminLogEventActionParticipantLeave : TLAbsChannelAdminLogEventAction
    {
        public override uint Constructor
        {
            get
            {
                return 0xf89777f2;
            }
        }



    }
}
