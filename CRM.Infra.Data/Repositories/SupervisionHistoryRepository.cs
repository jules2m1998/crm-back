using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<ICollection<SupervisionHistory>> GetAllActivateSupervisionAsync()
    {

        return (await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .GroupBy(sh => new { sh.SupervisedId })
            .Select(rr => rr.OrderByDescending(r => r.CreatedAt).FirstOrDefault())
            .ToListAsync())!;
    }

    public async Task<ICollection<SupervisionHistory>> GetAllSupervisedUserBySupervisorAsync(Guid userId)
    { 
        var s = await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .GroupBy(sh => new { sh.SupervisedId })
            .Select(rr => rr.OrderByDescending(r => r.CreatedAt).FirstOrDefault())
            .ToListAsync();

        return s.Where(r => r != null && r.SupervisorId == userId).ToList()!;
    }

    public async Task<ICollection<SupervisionHistory>> GetAllSupervisedUserBySupervisorAsync(Guid userId, string userName)
    {
        var s = await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .GroupBy(sh => new { sh.SupervisedId })
            .Select(rr => rr.OrderByDescending(r => r.CreatedAt).FirstOrDefault())
            .ToListAsync();

        return s.Where(r => r != null && r.SupervisorId == userId && r.Supervisor.Creator != null && r.Supervisor.Creator.UserName == userName).ToList() as ICollection<SupervisionHistory>;
    }

    public async Task<ICollection<SupervisionHistory>> GetSuperviseesHistoryAsync(Guid userId)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(sh => sh.SupervisedId == userId)
            .ToListAsync();
    }

    public async Task<ICollection<SupervisionHistory>> GetSuperviseesHistoryAsync(Guid userId, string userName)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.Creator)
            .Where(sh => sh.SupervisorId == userId && sh.Supervisor.Creator != null && sh.Supervisor.Creator.UserName == userName)
            .ToListAsync();
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
            .Reverse()
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
            .Reverse()
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<SupervisionHistory>> GetSupervisionHistory(Guid userId)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(sh => sh.SupervisorId == userId)
            .ToListAsync();
    }

    public async Task<ICollection<SupervisionHistory>> GetSupervisionHistory(Guid userId, string userName)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.Creator)
            .Where(sh => sh.SupervisorId == userId && sh.Supervisor.Creator != null && sh.Supervisor.Creator.UserName == userName)
            .ToListAsync();
    }

    public async Task<SupervisionHistory?> GetUserSupervisor(Guid userId)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(s => s.Supervised.Id == userId)
            .OrderBy(sh => sh.CreatedAt)
            .Reverse()
            .FirstOrDefaultAsync();
    }

    public async Task<SupervisionHistory?> GetUserSupervisor(Guid userId, string userName)
    {
        return await _dbSet
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervised)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(sh => sh.Supervisor)
            .ThenInclude(u => u.Creator)
            .Where(s => s.Supervised.Id == userId && s.Supervisor.Creator != null && s.Supervisor.Creator.UserName == userName)
            .OrderBy(sh => sh.CreatedAt)
            .Reverse()
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
