using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infra.Data.Repositories;

public class ProductStageRepository : IProductStageRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<ProductStage> Table => _context.ProductStages;

    public ProductStageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ProductStage stage)
    {
        await Table.AddAsync(stage);
        await _context.SaveChangesAsync();
        Table.Attach(stage);
    }

    public async Task<ICollection<ProductStage>> GetAllAsync() =>
        await Table.Where(x => x.DeletedAt == null).ToListAsync();

    public async Task<ProductStage?> GetOneAsync(Guid id) => 
        await Table
        .Where(x => x.DeletedAt == null)
        .FirstOrDefaultAsync(t => t.Id == id);

    public async Task UpdateAsync(ProductStage productStage)
    {
        Table.Update(productStage);
        await _context.SaveChangesAsync();
        Table.Attach(productStage);
    }

    public async Task DeleteAsync(ProductStage item)
    {
        var data = await _context
            .Products
            .Where(x => x.DeletedAt == null)
            .FirstOrDefaultAsync();
        if (data == null) return;
        await _context.SaveChangesAsync();
        Table.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<ProductStage> stages)
    {
        await Table.AddRangeAsync(stages);
        await _context.SaveChangesAsync();
        Table.AttachRange(stages);
    }

    public async Task<IEnumerable<ProductStage>> GetByProductAsync(Guid productId) => 
        await Table
        .Include(x => x.Responses)
        .Where(x => x.ProductId == productId && x.DeletedAt == null)
        .ToListAsync();

    public async Task<ProductStage?> GetFirstByProductAsync(Guid productId, CancellationToken cancellationToken) =>
        await Table
        .Include(x => x.Responses)
        .Where(x => x.ProductId == productId && x.DeletedAt == null)
        .OrderBy(x => x.StageLevel)
        .FirstOrDefaultAsync(cancellationToken);
}
