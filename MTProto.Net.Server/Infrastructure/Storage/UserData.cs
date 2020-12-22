using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IUserData
	{
		Dictionary<string, string> Headers { get; set; } 
		int Id { get; set; }
		string UserName { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string About { get; set; }
		string Phone { get; set; }
		long Access_Hash { get; set; }

		int Pts { get; set; }
		int Seq { get; set; }
		
	}
	public class UserData:IUserData
	{
		public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
		public int Id { get; set; } 
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string About { get; set; }
		public string Phone { get; set; }
		public long Access_Hash { get; set; }
		public int Pts { get; set; }
		public int Seq { get; set; }

	}
}
