using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;
using MTProto.NET.Schema.TL;

namespace MTProto.NET.Schema.Layer72
{
    /* 
	 * messages.getDialogs#191ba9c5 flags:# exclude_pinned:flags.0?true offset_date:int offset_id:int offset_peer:InputPeer limit:int = messages.Dialogs;
	 *
	 */
    [MTObject(0x191ba9c5)]
    public class TLGetDialogs : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x191ba9c5;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.True)]
        public bool ExcludePinned { get; set; }
        [MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.Null)]
        public int? FolderId { get; set; }
        [MTParameter(Order = 3)]
        public int OffsetDate { get; set; }
        [MTParameter(Order = 4)]
        public int OffsetId { get; set; }
        
        [MTParameter(Order = 5)]
        public TLAbsInputPeer OffsetPeer { get; set; }

        //[MTParameter(Order = 5)]
        //public TLInputPeerEmpty OffsetPeer { get; set; }


        //[MTParameter(Order = 6)]
        //public int Limit { get; set; }
        //[MTParameter(Order = 7)]
        //public int Hash { get; set; }


    }
}
