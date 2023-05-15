using CRM.Core.Business.Models.Contact;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.GetOneContact;

public record GetOneContactQuery(string UserName, Guid id): IRequest<ContactOutModel>;
