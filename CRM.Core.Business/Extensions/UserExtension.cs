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
        return new UserModel(user.Id, user.UserName ?? "", user.Email ?? "", user.FirstName, user.LastName, user.UserRoles.Select(ur => ur.Role.Name ?? "").ToList(), user.Picture, user.PhoneNumber, user.CreatedAt, user.UpdateAt, user.DeletedAt);
    }

    private static SkillModel SkillToSkillModel(Skill st)
    {
        return new SkillModel { EndDate = st.EndDate, StartDate = st.StartDate, IsCurrent = st.IsCurrent, Name = st.Name, Place = st.Place };
    }
}
