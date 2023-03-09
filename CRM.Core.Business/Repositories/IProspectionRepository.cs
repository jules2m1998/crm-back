using CRM.Core.Business.Models.Prospect;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IProspectionRepository
{
    Task<ICollection<Prospect>> GetManyAsync(ICollection<ProspectionInModel> models);
    Task<ICollection<Prospect>> GetAllAsync();
    Task<ICollection<Prospect>> GetManyAsync(ICollection<ProspectionInModel> models, string userName);
    Task<ICollection<Prospect>> GetProspectionByAgent(Guid agentId);
    Task<ICollection<Prospect>> GetProspectionByAgent(Guid agentId, string userName);
    Task<ICollection<Prospect>> GetProspectionByCompany(Guid companyId);
    Task<ICollection<Prospect>> GetProspectionByCompany(Guid companyId, string userName);
    Task<ICollection<Prospect>> GetProspectionByProduct(Guid productId);
    Task<ICollection<Prospect>> GetProspectionByProduct(Guid productId, string userName);
    Task<Prospect?> GetTheCurrentAsync(Guid productId, Guid companyId);
    Task<Prospect> SaveAsync(Prospect product);
    Task<ICollection<Prospect>> UpdateAsync(ICollection<Prospect> prospections);
    Task<Prospect?> GetOneAsync(Guid agentId, Guid productId, Guid companyId);
    Task<Prospect?> GetOneAsync(Guid agentId, Guid productId, Guid companyId, string creatorUserName);
}
