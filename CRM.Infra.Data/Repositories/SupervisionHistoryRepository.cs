using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class SupervisionHistoryRepository : ISupervisionHistoryRepository
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DbSet<SupervisionHistory> _dbSet;

    public SupervisionHistoryRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.SupervisionHistories;
    }

    public async Task<ICollection<SupervisionHistory>> AddRangeAsync(ICollection<SupervisionHistory> supervisionHistories)
    {
        _dbSet.AddRange(supervisionHistories);
        await _dbContext.SaveChangesAsync();
        _dbSet.AttachRange(supervisionHistories);
        return supervisionHistories;
    }

    public async Task<SupervisionHistory?> GetSupervisionAsync(Guid supervisorId, Guid supervisedId)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(sh => sh.SupervisorId == supervisorId && sh.SupervisedId == supervisedId)
            .OrderBy(sh => sh.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<SupervisionHistory?> GetSupervisionAsync(Guid supervisorId, Guid supervisedId, string creatorName)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(sh => sh.SupervisorId == supervisorId && sh.SupervisedId == supervisedId && sh.Supervisor.Creator != null && sh.Supervisor.Creator.UserName == creatorName)
            .OrderBy(sh => sh.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<SupervisionHistory> UpdateAsync(SupervisionHistory history)
    {
        _dbSet.Update(history);
        await _dbContext.SaveChangesAsync();
        _dbSet.Attach(history);

        return history;
    }
}
