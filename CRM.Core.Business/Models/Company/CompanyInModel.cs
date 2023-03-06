using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Types;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CRM.Core.Business.Models.Company;

public class CompanyInModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public IFormFile? Logo { get; set; }
    public IFormFile? CEOPicture { get; set; }
    [Required]
    public string CEOName { get; set; }
    [Required]
    public string Values { get; set; }
    [Required]
    public string Mission { get; set; }
    [Required]
    public string Concurrent { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public ActivityArea ActivityArea { get; set; }
    [Required]
    public CompaniesType Size { get; set; }

    public CompanyInModel()
    {
    }

    public CompanyInModel(string name, string description, IFormFile? logo, IFormFile? cEOPicture, string cEOName, string values, string mission, string concurrent, string location, ActivityArea activityArea, CompaniesType size)
    {
        Name = name;
        Description = description;
        Logo = logo;
        CEOPicture = cEOPicture;
        CEOName = cEOName;
        Values = values;
        Mission = mission;
        Concurrent = concurrent;
        Location = location;
        ActivityArea = activityArea;
        Size = size;
    }
}
