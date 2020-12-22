using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.Layer72
{
    /*
	 * peerNotifySettings#9acda4c0 
	 * flags:# 
	 * show_previews:flags.0?true 
	 * silent:flags.1?true 
	 * mute_until:int 
	 * sound:string = PeerNotifySettings;
	 
	 */
    [MTObject(0x9acda4c0)]
    public class TLPeerNotifySettings : MTObject
    {
        public override uint Constructor
        {
            get
            {
                return 0x9acda4c0;
            }
        }

        [MTParameter(Order = 0, IsFlag = true)]
        public int Flags { get; set; }
        [MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.Null)]
        public bool? ShowPreviews { get; set; }
        [MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.Null)]
        public bool? Silent { get; set; }
        [MTParameter(Order = 3, FlagBitId = 2, FlagType = FlagType.Null)]
        public int? MuteUntil { get; set; }
        [MTParameter(Order = 4, FlagBitId = 3, FlagType = FlagType.Null)]
        public string Sound { get; set; }

    }

    [MTObject(0xadd53cb3)]
    public class TLPeerNotifySettingsEmpty:MTObject
	{
        public override uint Constructor => 0xadd53cb3;

    }
}
