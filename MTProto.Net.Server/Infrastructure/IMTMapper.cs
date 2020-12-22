using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure
{
	public interface IMTMapper
	{
		Tout Map<Tin, Tout>(Tin input);
	}
}
