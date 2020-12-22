using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Services.Upload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Photos
{
	
	public class UploadProfilePhotoHandler : IProtoMessageHanddler<MTProto.NET.Schema.Layer72.TLUploadProfilePhoto>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;
		private readonly IUploadService uploadService;

		public UploadProfilePhotoHandler(ILogger<UploadProfilePhotoHandler> logger, IMTSessionManager manager, IUploadService service)
		{
			this.logger = logger;
			this.manager = manager;
			this.uploadService = service;
		}

		public async Task<MTObject> Handle(IMessageContext<MTProto.NET.Schema.Layer72.TLUploadProfilePhoto> context)
		{
			await Task.CompletedTask;

			var result = new MTProto.NET.Schema.TL.Photos.TLPhoto();
			result.Users = new TLVector<TLAbsUser>();
			try
			{
				var session = this.manager.GetSession(context.AuthKey());
				using (var store = session.Services.Store().GetUserStore())
				{
					var user = await store.GetUserById(session.GetUserId());
					result.Photo = new MTProto.NET.Schema.TL.TLPhotoEmpty
					{
						Id = 12

					};
					result.Users.Add(new MTProto.NET.Schema.Layer72.TLUser
					{
						Id = user.Id,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Self = true,
						Photo = new MTProto.NET.Schema.Layer72.TLUploadProfilePhoto
						{
							File = new MTProto.NET.Schema.Layer72.TLFileLocation
							{
								DcId =2,
								LocalId =10,
								Secret =879,
								VolumeId = 12
							}
						}
					}); ;
				}
			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to handle 'UploadProfilePhoto': \r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}
	}
}
