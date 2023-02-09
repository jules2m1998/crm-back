using System.ComponentModel.DataAnnotations;

namespace CRM.Core.Business.Models
{
    public class UserBodyAndRole: UserBody
    {
        [Required]
        public string Role { get; set; } = string.Empty;
        public ICollection<SkillModel>? Studies { get; set; }
        public ICollection<SkillModel>? Experiences { get; set;}
    }
}
