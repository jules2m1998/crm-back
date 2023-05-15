using CRM.Core.Business.Models.Contact;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.GetAllContacts;

public record GetAllContactsQuery(string UserName): IRequest<ICollection<ContactOutModel>>;
