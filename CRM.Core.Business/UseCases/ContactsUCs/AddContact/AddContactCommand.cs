using CRM.Core.Business.Models.Contact;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.AddContact;

public class AddContactCommand: ContactInModel, IRequest<ContactOutModel>
{
    public string UserName { get; set; } = string.Empty;
}
