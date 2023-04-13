using CRM.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class Company: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = DefaultParams.defaultCompanyPicture;
    public string CEOPicture { get; set; } = DefaultParams.defaultCEOPicture;
    public string CEOName { get; set; } = string.Empty;
    public string Values { get; set; } = string.Empty;
    public string Mission { get; set; } = string.Empty;
    public string Concurrent { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ActivityArea ActivityArea { get; set; }
    public CompaniesType Size { get; set; }

    public virtual ICollection<CompanyContact>? CompanyContacts { get; set; }
    public virtual ICollection<Prospect>? Prospections { get; set; }

    public Company()
    {
    }

    public Company(string name, string description, string cEOName, string values, string mission, string concurrent, string location, ActivityArea activityArea, CompaniesType size, User creator): base(creator)
    {
        Name = name;
        Description = description;
        CEOName = cEOName;
        Values = values;
        Mission = mission;
        Concurrent = concurrent;
        Location = location;
        ActivityArea = activityArea;
        Size = size;
    }
}
