using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Event;

public class EventOutModel
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ProspectionOutModel? Prospection { get; set; }
    public UserModel? Creator { get; set; }
    public UserModel Owner { get; set; } = null!;
    public ICollection<ContactOutModel>? Contacts { get; set; }
}
