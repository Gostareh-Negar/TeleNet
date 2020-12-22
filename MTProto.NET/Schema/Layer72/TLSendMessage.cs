using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;
using MTProto.NET.Schema.TL;

namespace MTProto.NET.Schema.Layer72
{
    /*
	 * messages.sendMessage#fa88427a 
	 * flags:# 
	 * no_webpage:flags.1?true 
	 * broadcast:flags.4?true 
	 * peer:InputPeer 
	 * reply_to_msg_id:flags.0?int 
	 * message:string 
	 * random_id:long 
	 * reply_markup:flags.2?ReplyMarkup 
	 * entities:flags.3?Vector<MessageEntity> = Updates;
	 */
    [MTObject(0xfa88427a)]
    public class TLSendMessage : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0xfa88427a;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 1, FlagType = FlagType.True)]
        public bool NoWebpage { get; set; }
        [MTParameter(Order = 2, FlagBitId = 5, FlagType = FlagType.True)]
        public bool Silent { get; set; }
        [MTParameter(Order = 3, FlagBitId = 6, FlagType = FlagType.True)]
        public bool Background { get; set; }
        [MTParameter(Order = 4, FlagBitId = 7, FlagType = FlagType.True)]
        public bool ClearDraft { get; set; }
        [MTParameter(Order = 5)]
        public TLAbsInputPeer Peer { get; set; }
        [MTParameter(Order = 6, FlagBitId = 0, FlagType = FlagType.Null)]
        public int? ReplyToMsgId { get; set; }
        [MTParameter(Order = 7)]
        public string Message { get; set; }
        [MTParameter(Order = 8)]
        public long RandomId { get; set; }
        [MTParameter(Order = 9, FlagBitId = 2, FlagType = FlagType.Null)]
        public TLAbsReplyMarkup ReplyMarkup { get; set; }
        
        [MTParameter(Order = 10, FlagBitId = 3, FlagType = FlagType.Null)]
        public TLVector<TLAbsMessageEntity> Entities { get; set; }
        
        //[MTParameter(Order = 11, FlagBitId = 10, FlagType = FlagType.Null)]
        //public int? ScheduleDate { get; set; }


    }
}
