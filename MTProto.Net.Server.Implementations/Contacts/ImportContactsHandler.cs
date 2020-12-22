using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL.Contacts;
using MTProto.NET.Schema.TL.Requests.Contacts;
using MTProto.NET.Server.Infrastructure;
using MTProto.NET.Server.Services.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Implementations.Contacts
{
	/*
	 * contacts.importContacts#2c800be5 contacts:Vector<InputContact> = contacts.ImportedContacts;
	 * Refernces:
	 *	1. contacts.importContacts#2c800be5 contacts:Vector<InputContact> = contacts.ImportedContacts;
	 *	2. https://core.telegram.org/method/contacts.importContacts
	 */
	public class ImportContactsHandler : IProtoMessageHanddler<TLImportContacts>
	{
		private readonly ILogger logger;

		private readonly IMTSessionManager manager;
		private readonly IContactService contactService;

		public ImportContactsHandler(ILogger<ImportContactsHandler> logger, IMTSessionManager manager, IContactService contactService)
		{
			this.logger = logger;
			this.manager = manager;
			this.contactService = contactService;
		}

		public async Task<MTObject> Handle(IMessageContext<TLImportContacts> context)
		{
			TLImportedContacts result = new TLImportedContacts();
			result.Users = new NET.Schema.TL.TLVector<NET.Schema.TL.TLAbsUser>();
			result.Imported = new NET.Schema.TL.TLVector<NET.Schema.TL.TLImportedContact>();
			result.RetryContacts = new NET.Schema.TL.TLVector<long>();
			result.PopularInvites = new NET.Schema.TL.TLVector<NET.Schema.TL.TLPopularContact>();
			try
			{
				var session = manager.GetSession(context.AuthKey());
				if (session == null)
					throw new Exception("Seesion not found.");
				var userId = session.GetUserId();
				var request = context.Body;
				foreach(var contact in request.Contacts)
				{
					if (contact.ClientId == 0)
						contact.ClientId = new Random().Next();
					//contact.ClientId++;
					var added_contact = await this.contactService.ImportContact(userId, contact.FirstName, contact.LastName, contact.Phone, contact.ClientId);
					if (added_contact != null)
					{
						result.Imported.Add(new NET.Schema.TL.TLImportedContact { 
							ClientId = added_contact.ClientId,
							UserId = added_contact.ImportedContactId
						});
						if (added_contact.ImportedContactId != 0)
						{
							result.Users.Add(new MTProto.NET.Schema.Layer72.TLUser { 
								Id = added_contact.ImportedContactId,
								FirstName = string.IsNullOrWhiteSpace(added_contact.FirstName)?$"User {added_contact.Id}":added_contact.FirstName,
								LastName = added_contact.LastName,
								Phone = added_contact.Phone
							});
						}

					}
				}


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
