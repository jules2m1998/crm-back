using CRM.Core.Domain.Types;

namespace CRM.Core.Domain.Entities;

public class Contact: BaseEntity
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public ContactVisibility Visibility { get; set; } = ContactVisibility.Public;
    public virtual ICollection<User> SharedTo { get; set; } = new List<User>();
    public virtual Company Company { get; set; } = null!;
    public virtual ICollection<PhoneNumber> Phones { get; set; } = new List<PhoneNumber>();
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
