using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;
using MTProto.NET.Schema.TL;

namespace MTProto.NET.Schema.Layer72
{
    /* 
	 * dialog#e4def5db 
	 * flags:# 
	 * pinned:flags.2?true 
	 * peer:Peer 
	 * top_message:int 
	 * read_inbox_max_id:int 
	 * read_outbox_max_id:int 
	 * unread_count:int 
	 * unread_mentions_count:int 
	 * notify_settings:PeerNotifySettings 
	 * pts:flags.0?int 
	 * draft:flags.1?DraftMessage = Dialog;
	 */
    [MTObject(0xe4def5db)]
    public class TLDialog : TLAbsDialog
    {
        public override uint Constructor
        {
            get
            {
                return 0xe4def5db;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 2, FlagType = FlagType.True)]
        public bool Pinned { get; set; }
        [MTParameter(Order = 2, FlagBitId = 3, FlagType = FlagType.True)]
        public bool UnreadMark { get; set; }
        [MTParameter(Order = 3)]
        public TLAbsPeer Peer { get; set; }
        [MTParameter(Order = 4)]
        public int TopMessage { get; set; }
        [MTParameter(Order = 5)]
        public int ReadInboxMaxId { get; set; }
        [MTParameter(Order = 6)]
        public int ReadOutboxMaxId { get; set; }
        [MTParameter(Order = 7)]
        public int UnreadCount { get; set; }
        [MTParameter(Order = 8)]
        public int UnreadMentionsCount { get; set; }
        [MTParameter(Order = 9)]
        public MTProto.NET.Schema.Layer72.TLPeerNotifySettingsEmpty NotifySettings { get; set; }
        [MTParameter(Order = 10, FlagBitId = 0, FlagType = FlagType.Null)]
        public int? Pts { get; set; }
        [MTParameter(Order = 11, FlagBitId = 1, FlagType = FlagType.Null)]
        public TLAbsDraftMessage Draft { get; set; }
     
        //[MTParameter(Order = 12, FlagBitId = 4, FlagType = FlagType.Null)]
        //public int? FolderId { get; set; }


    }
}
