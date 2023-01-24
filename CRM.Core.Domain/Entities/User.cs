using Microsoft.AspNetCore.Identity;

namespace CRM.Core.Domain.Entities;

public class User: IdentityUser<Guid>
{
    public string Picture { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
