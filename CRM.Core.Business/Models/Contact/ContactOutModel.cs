using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Models.PhoneNumber;
using CRM.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Contact;

public class ContactOutModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public ICollection<PhoneOutWithoutContact> Phones { get; set; } = new List<PhoneOutWithoutContact>();
    public ContactVisibility Visibility { get; set; } = ContactVisibility.Public;
    public ICollection<UserModel> SharedTo { get; set; } = new List<UserModel>();
    public CompanyOutModel Company { get; set; } = null!;
}
