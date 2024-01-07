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
        var stage = await Set.Where(x => x.Id.Equals(stageId)).FirstOrDefaultAsync();
        if (stage == null) return;
        stage.DeletedAt = DateTime.Now;
        Set.Update(stage);
        await _dbContext.SaveChangesAsync();
    }

    public override async Task<IReadOnlyList<ProductStage>> GetAllAsync()
    {
        return await Set
            .Where(x => x.DeletedAt != null)
            .ToListAsync();
    }

    public override async Task<ProductStage?> GetByIdAsync(Guid id)
    {
        return await Set.FirstOrDefaultAsync(x => x.DeletedAt != null && x.Id == id);
    }
}
