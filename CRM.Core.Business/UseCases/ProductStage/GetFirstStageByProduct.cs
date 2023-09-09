using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProductStage;

public static class GetFirstStageByProduct
{
    public record Query(Guid ProductId): IRequest<ProductStageModel.Out>;
    public class Handler : IRequestHandler<Query, ProductStageModel.Out>
    {
        private readonly IProductStageRepository repo;
        private readonly IMapper mapper;

        public Handler(IProductStageRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ProductStageModel.Out> Handle(Query request, CancellationToken cancellationToken)
        {
            Domain.Entities.ProductStage stage = await repo.GetFirstByProductAsync(request.ProductId, cancellationToken) ?? throw new NotFoundEntityException("This stage don't exist !");
            return mapper.Map<ProductStageModel.Out>(stage);
        }
    }
}
