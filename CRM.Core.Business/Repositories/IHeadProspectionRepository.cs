using CRM.Core.Business.Models.HeadProspectionModel;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IHeadProspectionRepository
{
    Task<int> AddAdync(HeadProspection head, CancellationToken cancellationToken);
    Task<int> DeleteAsync(HeadProspection data, CancellationToken cancellationToken);
    Task<ICollection<HeadProspection>> GetAllAsync(CancellationToken cancellationToken);
    Task<HeadProspection?> GetByIndexAsync(Guid productId, Guid companyId, Guid agentId, CancellationToken cancellationToken);
    Task UpdateAsync(HeadProspection data, CancellationToken cancellationToken);
}
