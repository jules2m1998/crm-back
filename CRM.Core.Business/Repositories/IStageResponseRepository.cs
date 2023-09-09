using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IStageResponseRepository
{
    Task<StageResponse> AddAsync(StageResponse response, CancellationToken cancellationToken);
    Task<StageResponse?> DeleteAsync(Guid id);
    Task<IEnumerable<StageResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<StageResponse>> GetAllAsync(Guid questionId, CancellationToken cancellationToken);
    Task<IEnumerable<StageResponse>> GetAllByStageResponseAsync(Guid productId, Guid? responseId, CancellationToken cancellationToken);
    Task<StageResponse?> GetByIdAsync(Guid id);
    Task<StageResponse?> UpdateAsync(Guid id, StageResponse model, CancellationToken cancellationToken);
}
