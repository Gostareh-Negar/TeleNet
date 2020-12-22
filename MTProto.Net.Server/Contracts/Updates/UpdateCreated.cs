using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Contracts.Updates
{
	public class UpdateCreated
	{
		public object Payload { get; set; }
		public int UserId { get; set; }
	}
}
