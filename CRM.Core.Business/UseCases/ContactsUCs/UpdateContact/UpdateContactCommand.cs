using CRM.Core.Business.Models.Contact;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.UpdateContact;

public record UpdateContactCommand(Guid Id, string UserName, ContactInModel Model): IRequest<ContactOutModel>;
