using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;

namespace MTProto.NET.Schema.Layer72
{
	/*
	 * userProfilePhoto#d559d8c8 photo_id:long photo_small:FileLocation photo_big:FileLocation = UserProfilePhoto;
	 * */
	[MTObject(0xd559d8c8)]
	public class TLUserProfilePhoto : MTObject
	{
		public override uint Constructor => 0xd559d8c8;

		[MTParameter(Order = 0, IsFlag = true)]
		public long FileId { get; set; }

		[MTParameter(Order = 1, IsFlag = true)]
		public TLFileLocation PhotoSmall { get; set; }
		[MTParameter(Order = 2, IsFlag = true)]

		public TLFileLocation PhotoBig { get; set; }


	}
}
