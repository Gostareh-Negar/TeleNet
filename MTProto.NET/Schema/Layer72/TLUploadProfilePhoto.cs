using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Schema.TL;
using MTProto.NET.Enums;

namespace MTProto.NET.Schema.Layer72
{
	/*
	 * 
	 * photos.uploadProfilePhoto#4f32c098 file:InputFile = photos.Photo;
	 * */
	[MTObject(0x4f32c098)]
	public class TLUploadProfilePhoto : TLAbsUserProfilePhoto
	{
		public override uint Constructor => 0x4f32c098;

		[MTParameter(Order = 0)]
		public TLAbsInputFile File { get; set; }

		//[MTParameter(Order = 0, IsFlag = true)]
		//public int Flags { get; set; }
		//[MTParameter(Order = 1, FlagBitId = 0, FlagType = FlagType.Null)]
		//public TLAbsInputFile File { get; set; }
		//[MTParameter(Order = 2, FlagBitId = 1, FlagType = FlagType.Null)]
		//public TLAbsInputFile Video { get; set; }
		//[MTParameter(Order = 3, FlagBitId = 2, FlagType = FlagType.Null)]
		//public double? VideoStartTs { get; set; }
	}
}
