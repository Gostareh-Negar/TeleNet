using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public interface IContactData
	{
		int Id { get; set; }
		int UserId { get; set; }
		int ImportedContactId { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string Phone { get; set; }
		long ClientId { get; set; }
	}
	public class ContactData : IContactData
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int ImportedContactId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public long ClientId { get; set; }

	}
}
