using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace CRM.api.Models
{
    public class User: IdentityUser<Guid>
    {
        public string Picture { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
