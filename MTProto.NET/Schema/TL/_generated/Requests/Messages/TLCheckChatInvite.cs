using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.TL.Requests.Messages
{
    [MTObject(0x3eadb1bb)]
    public class TLCheckChatInvite : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x3eadb1bb;
            }
        }

        [MTParameter(Order = 0)]
        public string Hash { get; set; }


    }
}
