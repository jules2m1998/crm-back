using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface ICommitRepository
{
    Task AddAsync(Commit commit, CancellationToken cancellationToken);
    Task<int> DeleteAsync(Commit commit, CancellationToken cancellationToken);
    Task<ICollection<Commit>> GetAllAsync(CancellationToken cancellationToken);
    Task<Commit?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Commit commit, CancellationToken cancellationToken);
}
