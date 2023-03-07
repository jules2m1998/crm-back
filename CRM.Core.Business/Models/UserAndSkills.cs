using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public class UserAndSkills: BaseUserModel
{
    virtual public ICollection<SkillModel>? Studies { get; set; }
    virtual public ICollection<SkillModel>? Experiences { get; set; }

    public UserAndSkills(
        Guid id,
        string userName,
        string email,
        string firstName,
        string lastName,
        List<string> roles,
        string? picture,
        string? phoneNumber,
        DateTime createdAt,
        DateTime? updateAt,
        DateTime? deletedAt,
        ICollection<SkillModel> studies,
        ICollection<SkillModel> experiences
        )
        : base(
            id,
            userName,
            email,
            firstName,
            lastName,
            roles,
            picture,
            phoneNumber,
            createdAt,
            updateAt,
            deletedAt)
    {
        Studies = studies;
        Experiences = experiences;
    }
}
