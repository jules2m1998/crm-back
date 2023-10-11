using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IStageRepository : IAsyncRepository<Domain.Entities.ProductStage>
{
    Task DeleteStageAsync(Guid stageId);
}
