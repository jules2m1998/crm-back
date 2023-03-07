using CRM.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public class UserUpdateModel
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public List<string> Roles { get; set; }
    public IFormFile? Picture { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NewPassword { get; set; }
    public string? OldPassword { get; set; }

    public ICollection<SkillModel>? Studies { get; set; }
    public ICollection<SkillModel>? Experiences { get; set; }

    public UserUpdateModel()
    {

    }

    public UserUpdateModel(
        Guid id,
        string userName,
        string email,
        string firstName,
        string lastName,
        List<string> roles,
        IFormFile? picture,
        string? phoneNumber,
        string? newPassword,
        string? oldPassword,
        ICollection<SkillModel>? studies,
        ICollection<SkillModel>? experiences
        )
    {

        Id = id;
        UserName = userName;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
        Picture = picture;
        PhoneNumber = phoneNumber;
        Studies = studies;
        Experiences = experiences;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    public void CopyToUser(User u)
    {
        u.UserName = UserName;
        u.Email = Email;
        u.FirstName = FirstName;
        u.LastName = LastName;
        u.PhoneNumber = PhoneNumber;
        if(Studies is null)
            u.Studies = new List<Skill>();
        else
            u.Studies = Studies.Select(s => new Skill(s.Name, s.Place, s.StartDate, s.EndDate, s.IsCurrent, null, null)).ToList();
        if(Experiences is not null) 
            u.Experiences = Experiences.Select(s => new Skill(s.Name, s.Place, s.StartDate, s.EndDate, s.IsCurrent, null, null)).ToList();
        else 
            u.Experiences = new List<Skill>();
    }
}
