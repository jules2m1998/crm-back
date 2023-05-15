using CRM.Core.Business.Models.Contact;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.DeleteContact;

public record DeleteContactCommand(Guid Id, string UserName): IRequest;