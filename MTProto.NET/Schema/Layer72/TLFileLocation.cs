using System;
using System.Collections.Generic;
using System.Text;
using MTProto.NET.Attributes;
using MTProto.NET.Schema.TL;

namespace MTProto.NET.Schema.Layer72
{
	/*
	 *  fileLocation#53d69076 dc_id:int volume_id:long local_id:int secret:long = FileLocation;
	 */

	[MTObject(0x53d69076)]
	public class TLFileLocation : TLAbsInputFile
	{
		public override uint Constructor => 0x53d69076;

		[MTParameter(Order =0)]
		public int DcId { get; set; }
		[MTParameter(Order = 1)]
		public long VolumeId { get; set; }
		[MTParameter(Order = 2)]

		public int LocalId { get; set; }
		[MTParameter(Order = 3)]

		public long Secret { get; set; }
	}
}
