using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IUpdateData
	{
		int Id { get; set; }
		byte[] Content { get; set; }
	}

	public class UpdateData : IUpdateData 
	{
		public int Id { get; set; }
		public byte[] Content { get; set; }

	}

}
