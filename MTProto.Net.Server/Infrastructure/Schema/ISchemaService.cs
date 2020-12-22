using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MTProto.NET.Server.Schema
{
	public class MTSchema
	{
		public Type Type { get; private set; }
		public uint Constructor { get; private set; }

		internal MTSchema(Type type, uint constructor)
		{
			this.Type = type;
			this.Constructor = constructor;
		}
		
	}
	public interface ISchemaService
	{
		IEnumerable<MTSchema> GetAll();
	}
	class SchemaService : ISchemaService
	{
		public IEnumerable<MTSchema> GetAll()
		{
			return MTProto.NET.MTContext.Types.Select(x => new MTSchema(x.Value, x.Key));
		}
	}
}
