using CRM.Core.Business.Repositories;
using CRM.Core.Business.Responses;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class StageRepository : BaseRepository<ProductStage>, IStageRepository
{
    public StageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task DeleteStageAsync(Guid stageId)
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var stage = await Set.Where(x => x.Id.Equals(stageId)).FirstOrDefaultAsync();
            if (stage == null) return;
            Set.Remove(stage);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }
}
