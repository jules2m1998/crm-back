using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public class UserAndCreatorModel: BaseUserModel
{
    public BaseUserModel Creator { get; set; }

    public UserAndCreatorModel(
        Guid id,
        string userName,
        string email,
        string firstName,
        string lastName,
        bool isActivated,
        List<string> roles,
        string? picture, 
        string? phoneNumber,
        DateTime createdAt, 
        DateTime? updateAt, 
        DateTime? deletedAt, 
        BaseUserModel creator
        ): base(id, userName, email, firstName, lastName, isActivated, roles, picture, phoneNumber, createdAt, updateAt, deletedAt)
    {
        Creator = creator;
    }
}
