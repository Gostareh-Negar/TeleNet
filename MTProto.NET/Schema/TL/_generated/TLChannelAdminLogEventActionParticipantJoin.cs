using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL
{
    [MTObject(0x183040d3)]
    public class TLChannelAdminLogEventActionParticipantJoin : TLAbsChannelAdminLogEventAction
    {
        public override uint Constructor
        {
            get
            {
                return 0x183040d3;
            }
        }



    }
}
