using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public class BaseUserCsvModel
{
    public Guid? Id { get; set; }
    [Required]
    public string? UserName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string? Email { get; set; } = string.Empty;
    [Required]
    public string? FirstName { get; set; } = string.Empty;
    [Required]
    public string? LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public List<string>? Roles { get; set; } = new List<string>();
    public string? Picture { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public class BaseUserModelMapper<T> : ClassMap<T> where T : BaseUserCsvModel
{
    public BaseUserModelMapper()
    {
        Map(m => m.UserName).Name("id");
        Map(m => m.Email).Name("email");
        Map(m => m.FirstName).Name("firstname");
        Map(m => m.LastName).Name("lastname");
        Map(m => m.PhoneNumber).Name("phone_number");
    }
}