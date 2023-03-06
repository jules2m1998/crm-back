using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DbSet<Product> _products;

    private IQueryable<Product> _includeCreator { get { return _products.Include(p => p.Creator).Where(p => p.DeletedAt == null); } }

    public ProductRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _products = _dbContext.Products;
    }

    public async Task<Product> CreateOneAsync(Product product)
    {
        await _products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        _products.Attach(product);
        return product;
    }

    public async Task<ICollection<Product>> GetAllAsync()
    {
        return await _includeCreator
            .ToListAsync();
    }

    public async Task<ICollection<Product>> GetAllByCreator(string userName)
    {
        return await _includeCreator
                .Where(p => p.Creator != null && p.Creator.UserName == userName)
                .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAndCreatorAsync(Guid id, string userName)
    {
        return await _includeCreator.FirstOrDefaultAsync(p => p.Id == id && p.Creator != null && p.Creator.UserName == userName);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _includeCreator.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> PathProductAsync(JsonPatchDocument<Product> pathData, Product product)
    {
        pathData.ApplyTo(product);
        product.UpdateAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task DeleteManyAsync(ICollection<Product> products)
    {
        _products.RemoveRange(products);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Product>> GetManyByIdsAsync(ICollection<Guid> ids)
    {
        return await _includeCreator.Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public async Task<ICollection<Product>> GetManyByIdsAsync(ICollection<Guid> ids, string userName)
    {
        return await _includeCreator.Where(p => ids.Contains(p.Id) && p.Creator != null && p.Creator.UserName == userName).ToListAsync();
    }

    public async Task MarkAsDeletedAsync(ICollection<Product> products)
    {
        foreach (var product in products)
        {
            product.DeletedAt = DateTime.UtcNow;
        }
        _products.UpdateRange(products);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Product> UpdateOneAsync(Product product)
    {
        product.UpdateAt = DateTime.UtcNow;
        _products.Update(product);
        await _dbContext.SaveChangesAsync();
        _products.Attach(product);
        return product;
    }
}
