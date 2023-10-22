using CRM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.UpdateEvent;

public class UpdateEventCommandOwnerDto
{
    public Guid Id { get; set; }
    public string Picture { get; set; } = DefaultParams.defaultUserPicture;
    public string FirstName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActivated { get; set; } = true;
}
