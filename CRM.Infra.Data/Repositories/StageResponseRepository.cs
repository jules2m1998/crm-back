using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infra.Data.Repositories;

public class StageResponseRepository : IStageResponseRepository
{
    private readonly ApplicationDbContext dbContext;
    private DbSet<StageResponse> StageResponses => dbContext.StageResponses;
    private IQueryable<StageResponse> Included => 
        StageResponses.Include(x => x.Stage).Include(x => x.NextStage);

    public StageResponseRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<StageResponse> AddAsync(StageResponse response, CancellationToken cancellationToken)
    {
        await StageResponses.AddAsync(response, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(response.Id) ?? response;
    }

    public async Task<StageResponse?> GetByIdAsync(Guid id) => 
        await Included.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<StageResponse>> GetAllAsync(CancellationToken cancellationToken) => 
        await Included.ToListAsync(cancellationToken);



    public async Task<IEnumerable<StageResponse>> GetAllAsync(Guid questionId, CancellationToken cancellationToken) =>
        await Included.Include(x => x.NextStage).ToListAsync(cancellationToken);

    public async Task<StageResponse?> UpdateAsync(Guid id, StageResponse model, CancellationToken cancellationToken)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) return null;
        existing.Response = model.Response;
        existing.NextStage = model.NextStage;
        existing.UpdateAt = DateTime.UtcNow;

        _ = await dbContext.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<StageResponse?> DeleteAsync(Guid id)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) return null;
        StageResponses.Remove(existing);
        _ = await dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<IEnumerable<StageResponse>> GetAllByStageResponseAsync(Guid productId, Guid? responseId, CancellationToken cancellationToken)
    {
        var query = dbContext.ProductStages.Where(x => x.ProductId == productId);
        if(responseId is null)
        {
            query = query.OrderBy(x => x.StageLevel);
        }
        else
        {
            var currentResponse = await dbContext.StageResponses.Include(x => x.NextStage).Where(x => x.Id == responseId).FirstAsync(cancellationToken);
            query = query.Where(x => currentResponse.NextStage != null && x.StageLevel > currentResponse.NextStage.StageLevel).OrderBy(x => x.StageLevel);
        }
        var currentStage = await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return await StageResponses
            .Where(x => currentStage != null && x.Stage.Id == currentStage.Id)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
