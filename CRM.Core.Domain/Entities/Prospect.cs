using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class Prospect
{
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public bool IsActivated { get; set; } = true;
    public virtual Product Product { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
    public virtual User Agent { get; set; } = null!;
    public virtual User? Creator { get; set; } = null!;

    public Prospect()
    {
    }

    public Prospect(Product product, Company company, User agent, User? creator)
    {
        Product = product;
        Company = company;
        Agent = agent;
        Creator = creator;
    }
}
