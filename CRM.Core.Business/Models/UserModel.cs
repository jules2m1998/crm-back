using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public record UserModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public string? Picture { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Token { get; set; } = string.Empty;

    public UserModel()
    {

    }

    public UserModel(Guid id, string userName, string email, string firstName, string lastName, List<string> roles, string? picture, string? phoneNumber, DateTime createdAt, DateTime? updateAt, DateTime? deletedAt, string token = "")
    {
        Id = id;
        UserName = userName;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
        Picture = picture;
        PhoneNumber = phoneNumber;
        CreatedAt = createdAt;
        UpdateAt = updateAt;
        DeletedAt = deletedAt;
        Token = token;
    }
}
