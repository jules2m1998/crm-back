using CRM.Core.Business.Models.Supervision;
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
    Task<ICollection<SupervisionHistory>> GetAllActivateSupervisionAsync();
    Task<ICollection<SupervisionHistory>> GetAllSupervisedUserBySupervisorAsync(Guid userId);
    Task<ICollection<SupervisionHistory>> GetAllSupervisedUserBySupervisorAsync(Guid userId, string userName);
    Task<ICollection<SupervisionHistory>> GetSuperviseesHistoryAsync(Guid userId);
    Task<ICollection<SupervisionHistory>> GetSuperviseesHistoryAsync(Guid userId, string userName);
    Task<SupervisionHistory?> GetSupervisionAsync(Guid supervisorId, Guid supervisedId);
    Task<SupervisionHistory?> GetSupervisionAsync(Guid supervisorId, Guid supervisedId, string creatorName);
    Task<ICollection<SupervisionHistory>> GetSupervisionHistory(Guid userId);
    Task<ICollection<SupervisionHistory>> GetSupervisionHistory(Guid userId, string userName);
    Task<SupervisionHistory?> GetUserSupervisor(Guid userId);
    Task<SupervisionHistory?> GetUserSupervisor(Guid userId, string userName);
    Task<SupervisionHistory> UpdateAsync(SupervisionHistory history);
}
