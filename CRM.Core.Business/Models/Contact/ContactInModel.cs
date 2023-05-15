using CRM.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Contact;

public class ContactInModel
{
    [Required]
    public Guid CompanyId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [EmailAddress, Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Job { get; set; } = string.Empty;
    [Required]
    public ICollection<string> Phones { get; set; } = new List<string>();
    public ContactVisibility Visibility { get; set; } = ContactVisibility.Private;
    public ICollection<Guid>? SharedTo { get; set;}
}
