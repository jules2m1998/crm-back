using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Types;

namespace CRM.Core.Business.Models.Company;

public class CompanyOutModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Logo { get; set; }
    public string CEOPicture { get; set; }
    public string CEOName { get; set; }
    public string Values { get; set; }
    public string Mission { get; set; }
    public string Concurrent { get; set; }
    public string Location { get; set; }
    public ActivityArea ActivityArea { get; set; }
    public CompaniesType Size { get; set; }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public bool IsActivated { get; set; }
    public UserModel? Creator { get; set; }

    public CompanyOutModel(string name, string description, string logo, string cEOPicture, string cEOName, string values, string mission, string concurrent, string location, ActivityArea activityArea, CompaniesType size, Guid id, DateTime createdAt, DateTime? updateAt, bool isActivated, UserModel? creator)
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
        Id = id;
        CreatedAt = createdAt;
        UpdateAt = updateAt;
        IsActivated = isActivated;
        Creator = creator;
    }
}
