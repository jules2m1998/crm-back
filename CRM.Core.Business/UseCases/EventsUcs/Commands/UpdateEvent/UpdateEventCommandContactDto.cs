using CRM.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.UpdateEvent;

public class UpdateEventCommandContactDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public ContactVisibility Visibility { get; set; } = ContactVisibility.Public;
    virtual public bool IsActivated { get; set; } = true;
}
