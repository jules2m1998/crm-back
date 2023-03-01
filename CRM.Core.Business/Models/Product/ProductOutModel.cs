using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Product; 
public class ProductOutModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Logo { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public UserModel? Creator { get; set; }

    public ProductOutModel(Guid id, string name, string? logo, string description, DateTime createdAt, DateTime? updatedAt, UserModel? creator)
    {
        Id = id;
        Name = name;
        Logo = logo;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Creator = creator;
    }
}
