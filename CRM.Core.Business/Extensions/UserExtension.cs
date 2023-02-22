using CRM.Core.Business.Models;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRM.Core.Business.Extensions;

public static class UserExtension
{
    public static UserModel ToUserModel(this User user)
    {
        return new UserModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            UserName = user.UserName!,
            LastName = user.LastName,
            Email = user.Email!,
            Studies = user.Studies!.Select(st => SkillToSkillModel(st)).ToList(),
            Experiences = user.Experiences!.Select(st => SkillToSkillModel(st)).ToList(),
            IsActivated= user.IsActivated,
            Roles = user.UserRoles.Select(ur => ur.Role.Name ?? "").ToList(),
            Picture = user.Picture,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            UpdateAt = user.UpdateAt,
            DeletedAt = user.DeletedAt
        };
    }

    private static SkillModel SkillToSkillModel(Skill st)
    {
        return new SkillModel { EndDate = st.EndDate, StartDate = st.StartDate, IsCurrent = st.IsCurrent, Name = st.Name, Place = st.Place };
    }
}
