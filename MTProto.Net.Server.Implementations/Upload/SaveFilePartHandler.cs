using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL;
using MTProto.NET.Schema.TL.Requests.Upload;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Services.Upload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Upload
{
	
	public class SaveFilePartHandler : IProtoMessageHanddler<TLSaveFilePart>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;
		private readonly IUploadService uploadService;

		public SaveFilePartHandler(ILogger<SaveFilePartHandler> logger, IMTSessionManager manager, IUploadService service)
		{
			this.logger = logger;
			this.manager = manager;
			this.uploadService = service;
		}

		public async Task<MTObject> Handle(IMessageContext<TLSaveFilePart> context)
		{
			await Task.CompletedTask;
			var result = new TLVector<bool>();
			try
			{
			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to handle 'SaveFilePart': \r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}
	}
}
