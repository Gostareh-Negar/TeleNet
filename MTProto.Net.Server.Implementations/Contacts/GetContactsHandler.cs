using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL.Contacts;
using MTProto.NET.Schema.TL.Requests.Contacts;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Services.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MTProto.NET.Schema.TL;

namespace MTProto.NET.Server.Implementations.Contacts
{
	/*
	 * contacts.getContacts#c023849f hash:int = contacts.Contacts;
	 */
	
	public class GetContactsHandler : IProtoMessageHanddler<TLGetContacts>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;
		private readonly IContactService contactService;

		public GetContactsHandler(ILogger<GetContactsHandler> logger, IMTSessionManager manager, IContactService contactService)
		{
			this.logger = logger;
			this.manager = manager;
			this.contactService = contactService;
		}

		public async Task<MTObject> Handle(IMessageContext<TLGetContacts> context)
		{
			var result = new TLContacts();
			result.Users = new TLVector<TLAbsUser>();
			result.Contacts = new TLVector<TLContact>();
			try
			{
				var session = manager.GetSession(context.AuthKey());
				if (session == null)
					throw new Exception("Seesion not found.");
				var userId = session.GetUserId();
				var request = context.Body;
				var contacts = await contactService.GetContacts(userId);
				contacts.ToList().ForEach(x => {
					result.Contacts.Add(new TLContact { 
						UserId = x.ImportedContactId,
					});
					result.Users.Add(new MTProto.NET.Schema.Layer72.TLUser { 
						Id = x.ImportedContactId,
						FirstName = x.FirstName,
						LastName = x.LastName,
						Phone = x.Phone,
						AccessHash = 5
					});
				});
			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to handle 'ImportContacts': \r\n{0}", err.GetBaseException().Message);
				throw;
			}
			return result;
		}
	}
}
