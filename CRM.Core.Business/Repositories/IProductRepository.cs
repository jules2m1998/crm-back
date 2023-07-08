using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IProductRepository
{
    Task<Product> CreateOneAsync(Product product);
    Task DeleteManyAsync(ICollection<Product> products);
    Task<ICollection<Product>> GetAllAsync();
    Task<ICollection<Product>> GetAllByCreator(string userName);
    Task<Product?> GetByNameAsync(string name);
    Task<ICollection<Product>> GetManyByIdsAsync(ICollection<Guid> ids);
    Task<ICollection<Product>> GetManyByIdsAsync(ICollection<Guid> ids, string userName);
    Task<Product?> GetProductByIdAndCreatorAsync(Guid id, string userName);
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product?> GetWithStageAsync(Guid productId);
    Task MarkAsDeletedAsync(ICollection<Product> products);
    Task<Product> PathProductAsync(JsonPatchDocument<Product> pathData, Product product);
    Task<Product> UpdateOneAsync(Product product);
}
