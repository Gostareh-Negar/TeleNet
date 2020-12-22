using MTProto.NET.Schema.TL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.UpdateProcessing
{
	public delegate Task ProcessUpdate(IUpdateProcessingContext context, TLAbsUpdate update, ProcessUpdate next);
	interface IUpdateProcessorMiddleware
	{
		Task HandleUpdate(IUpdateProcessingContext context, TLAbsUpdate update, ProcessUpdate next);
	}
}
