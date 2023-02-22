using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CRM.Core.Domain.Entities;

public class User: IdentityUser<Guid>
{
    public string Picture { get; set; } = DefaultParams.defaultUserPicture;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActivated { get; set; } = true;

    public virtual User? Creator { get; set; } = null!;
    public virtual ICollection<Skill>? Studies { get; set; }
    public virtual ICollection<Skill>? Experiences { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
}
