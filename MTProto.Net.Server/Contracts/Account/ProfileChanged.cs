using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Contracts.Account
{
	public class ProfileChanged
	{
		public int AccountId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }
	}
}
