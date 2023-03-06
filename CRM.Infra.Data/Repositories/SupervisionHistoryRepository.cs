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
}
