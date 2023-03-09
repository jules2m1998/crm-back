using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Prospect;

public class ProspectionOutModel
{
    public DateTime CreatedAt { get; set; }
    public ProductOutModel Product { get; set; }
    public CompanyOutModel Company { get; set; }
    public UserModel Agent { get; set; }
    public UserModel? Creator { get; set; }
    public DateTime? UpdateAt { get; set; }
    public bool IsActivated { get; set; }

    public ProspectionOutModel(DateTime createdAt, ProductOutModel product, CompanyOutModel company, UserModel agent, UserModel? creator, DateTime? updateAt, bool isActivated)
    {
        CreatedAt = createdAt;
        Product = product;
        Company = company;
        Agent = agent;
        Creator = creator;
        UpdateAt = updateAt;
        IsActivated = isActivated;
    }
}
