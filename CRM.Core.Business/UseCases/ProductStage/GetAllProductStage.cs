using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProductStage;

public static class GetAllProductStage
{
    public record Query(): IRequest<ICollection<ProductStageModel.Out>>;
    public class Handler : IRequestHandler<Query, ICollection<ProductStageModel.Out>>
    {
        private readonly IProductStageRepository _repo;

        public Handler(IProductStageRepository repo)
        {
            _repo = repo;
        }

        public async Task<ICollection<ProductStageModel.Out>> Handle(Query request, CancellationToken cancellationToken)
        {
            ICollection<Domain.Entities.ProductStage> data = await _repo.GetAllAsync();
            return data.Select(d => d.ToModel()).ToList();
        }
    }
}
