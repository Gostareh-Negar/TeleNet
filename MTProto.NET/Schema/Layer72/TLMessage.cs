using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;
using MTProto.NET.Schema.TL;

namespace MTProto.NET.Schema.Layer72
{
    /* 
	 * message#44f9b43d 
	 * flags:# 
	 * out:flags.1?true 
	 * mentioned:flags.4?true 
	 * media_unread:flags.5?true 
	 * silent:flags.13?true 
	 * post:flags.14?true 
	 * from_scheduled:flags.18?true 
	 * id:int from_id:flags.8?int 
	 * to_id:Peer 
	 * fwd_from:flags.2?MessageFwdHeader 
	 * via_bot_id:flags.11?int 
	 * reply_to_msg_id:flags.3?int 
	 * date:int 
	 * message:string 
	 * media:flags.9?MessageMedia 
	 * reply_markup:flags.6?ReplyMarkup 
	 * entities:flags.7?Vector<MessageEntity> 
	 * views:flags.10?int 
	 * edit_date:flags.15?int 
	 * post_author:flags.16?string 
	 * grouped_id:flags.17?long = Message;
	 */
    [MTObject(0x44f9b43d)]
    public class TLMessage : TLAbsMessage
    {
        public override uint Constructor
        {
            get
            {
                return 0x44f9b43d;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 1, FlagType = FlagType.True)]
        public bool Out { get; set; }
        [MTParameter(Order = 2, FlagBitId = 4, FlagType = FlagType.True)]
        public bool Mentioned { get; set; }
        [MTParameter(Order = 3, FlagBitId = 5, FlagType = FlagType.True)]
        public bool MediaUnread { get; set; }
        [MTParameter(Order = 4, FlagBitId = 13, FlagType = FlagType.True)]
        public bool Silent { get; set; }
        [MTParameter(Order = 5, FlagBitId = 14, FlagType = FlagType.True)]
        public bool Post { get; set; }
        [MTParameter(Order = 6, FlagBitId = 18, FlagType = FlagType.True)]
        public bool FromScheduled { get; set; }
        [MTParameter(Order = 7, FlagBitId = 19, FlagType = FlagType.True)]
        public bool Legacy { get; set; }
        [MTParameter(Order = 8, FlagBitId = 21, FlagType = FlagType.True)]
        public bool EditHide { get; set; }
        [MTParameter(Order = 9)]
        public int Id { get; set; }
        [MTParameter(Order = 10, FlagBitId = 8, FlagType = FlagType.Null)]
        public int? FromId { get; set; }
        [MTParameter(Order = 11)]
        public TLAbsPeer ToId { get; set; }
        [MTParameter(Order = 12, FlagBitId = 2, FlagType = FlagType.Null)]
        public MTProto.NET.Schema.TL.TLMessageFwdHeader FwdFrom { get; set; }
        [MTParameter(Order = 13, FlagBitId = 11, FlagType = FlagType.Null)]
        public int? ViaBotId { get; set; }
        [MTParameter(Order = 14, FlagBitId = 3, FlagType = FlagType.Null)]
        public int? ReplyToMsgId { get; set; }
        [MTParameter(Order = 15)]
        public int Date { get; set; }
        [MTParameter(Order = 16)]
        public string Message { get; set; }
        [MTParameter(Order = 17, FlagBitId = 9, FlagType = FlagType.Null)]
        public TLAbsMessageMedia Media { get; set; }
        [MTParameter(Order = 18, FlagBitId = 6, FlagType = FlagType.Null)]
        public TLAbsReplyMarkup ReplyMarkup { get; set; }
        [MTParameter(Order = 19, FlagBitId = 7, FlagType = FlagType.Null)]
        public TLVector<TLAbsMessageEntity> Entities { get; set; }
        [MTParameter(Order = 20, FlagBitId = 10, FlagType = FlagType.Null)]
        public int? Views { get; set; }
        [MTParameter(Order = 21, FlagBitId = 15, FlagType = FlagType.Null)]
        public int? EditDate { get; set; }
        [MTParameter(Order = 22, FlagBitId = 16, FlagType = FlagType.Null)]
        public string PostAuthor { get; set; }
        [MTParameter(Order = 23, FlagBitId = 17, FlagType = FlagType.Null)]
        public long? GroupedId { get; set; }
        [MTParameter(Order = 24, FlagBitId = 22, FlagType = FlagType.Null)]
        public TLVector<MTProto.NET.Schema.TL.TLRestrictionReason> RestrictionReason { get; set; }


    }
}
