using CRM.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public class UserModel : BaseUserModel
{
    public string Token { get; set; } = string.Empty;
    public UserModel()
    {
    }

    public UserModel(Guid id, string userName, string email, string firstName, string lastName, List<string> roles, string? picture, string? phoneNumber, DateTime createdAt, DateTime? updateAt, DateTime? deletedAt, string token = ""): base(id, userName, email, firstName, lastName, roles, picture, phoneNumber, createdAt, updateAt, deletedAt)
    {
        Token = token;
    }
}
