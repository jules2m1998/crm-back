using CRM.Core.Business.Models;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IProductStageRepository
{
    Task AddRangeAsync(IEnumerable<ProductStage> stages);
    Task CreateAsync(ProductStage stage);
    Task DeleteAsync(ProductStage item);
    Task<ICollection<ProductStage>> GetAllAsync();
    Task<IEnumerable<ProductStage>> GetByProductAsync(Guid productId);
    Task<ProductStage?> GetFirstByProductAsync(Guid productId, CancellationToken cancellationToken);
    Task<ProductStage?> GetOneAsync(Guid id);
    Task UpdateAsync(ProductStage productStage);
}
