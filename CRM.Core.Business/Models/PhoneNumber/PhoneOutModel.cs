using CRM.Core.Business.Models.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.PhoneNumber;

public class PhoneOutModel: PhoneOutWithoutContact
{
    public ContactOutModel Contact { get; set; } = null!;
}

public class PhoneOutWithoutContact
{
    public string Value { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
