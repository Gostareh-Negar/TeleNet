using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IUserUpdateData
	{
		int Id { get; set; }

		int UserId { get; set; }
		int Pts { get; set; }
		int PtsCount { get; set; }
		byte[] Content { get; set; }
	}
	public class UserUpdateData : IUserUpdateData
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public int Pts { get; set; }
		public int PtsCount { get; set; }
		public byte[] Content { get; set; }

	}
}
