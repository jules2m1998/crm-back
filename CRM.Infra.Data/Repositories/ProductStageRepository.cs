using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        await Table.ToListAsync();

    public async Task<ProductStage?> GetOneAsync(Guid id) => 
        await Table.FirstOrDefaultAsync(t => t.Id == id);

    public async Task UpdateAsync(ProductStage productStage)
    {
        Table.Update(productStage);
        await _context.SaveChangesAsync();
        Table.Attach(productStage);
    }

    public async Task DeleteAsync(ProductStage item)
    {
        var data = await _context.Products.Include(p => p.FirstStage).FirstOrDefaultAsync(p => p.FirstStage != null && p.FirstStage.Id == item.Id);
        if (data == null) return;
        data.FirstStage = null;
        await _context.SaveChangesAsync();
        Table.Remove(item);
        await _context.SaveChangesAsync();
    }
}
