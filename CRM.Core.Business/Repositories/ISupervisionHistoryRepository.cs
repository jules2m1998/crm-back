using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface ISupervisionHistoryRepository
{
    Task<ICollection<SupervisionHistory>> AddRangeAsync(ICollection<SupervisionHistory> supervisionHistories);
    Task<SupervisionHistory?> GetSupervisionAsync(Guid supervisorId, Guid supervisedId);
    Task<SupervisionHistory?> GetSupervisionAsync(Guid supervisorId, Guid supervisedId, string creatorName);
    Task<SupervisionHistory> UpdateAsync(SupervisionHistory history);
}
