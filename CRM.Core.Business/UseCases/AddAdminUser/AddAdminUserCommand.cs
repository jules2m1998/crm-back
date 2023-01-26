using CRM.Core.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CRM.Core.Business.UseCases.AddUser
{
    public class AddAdminUserCommand: IRequest<UserModel>
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public IFormFile? Picture { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
