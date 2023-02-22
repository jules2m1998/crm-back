using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models
{
    public class BaseUserModel
    {
        virtual public Guid Id { get; set; }
        virtual public string UserName { get; set; } = string.Empty;
        virtual public string Email { get; set; } = string.Empty;
        virtual public string FirstName { get; set; } = string.Empty;
        virtual public string LastName { get; set; } = string.Empty;
        virtual public List<string> Roles { get; set; } = new List<string>();
        virtual public string? Picture { get; set; }
        virtual public string? PhoneNumber { get; set; }
        virtual public DateTime CreatedAt { get; set; }
        virtual public DateTime? UpdateAt { get; set; }
        virtual public DateTime? DeletedAt { get; set; }
        virtual public bool IsActivated { get; set; } = true;

        public BaseUserModel()
        {

        }

        public BaseUserModel(Guid id, string userName, string email, string firstName, string lastName, List<string> roles, string? picture, string? phoneNumber, DateTime createdAt, DateTime? updateAt, DateTime? deletedAt)
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
        }
    }
}
