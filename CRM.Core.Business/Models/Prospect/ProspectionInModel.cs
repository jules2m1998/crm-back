using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Prospect;

public class ProspectionInModel
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public Guid CompanyId { get; set; }
    [Required]
    public Guid AgentId { get; set; }

    public ProspectionInModel(Guid productId, Guid companyId, Guid agentId)
    {
        ProductId = productId;
        CompanyId = companyId;
        AgentId = agentId;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProspectionInModel model &&
               ProductId.Equals(model.ProductId) &&
               CompanyId.Equals(model.CompanyId) &&
               AgentId.Equals(model.AgentId);
    }

    public override string? ToString()
    {
        return $"ProductId: {ProductId}; CompanyId: {CompanyId}; AgentId: {AgentId}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ProductId, CompanyId, AgentId);
    }
}
