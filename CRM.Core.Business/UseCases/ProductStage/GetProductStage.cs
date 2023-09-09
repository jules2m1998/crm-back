using AutoMapper;
using CRM.Core.Business.Extensions;
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

public static class GetProductStage
{
    public record Query(Guid Id): IRequest<ProductStageModel.Out>;
    public class Handler : IRequestHandler<Query, ProductStageModel.Out>
    {
        private readonly IProductStageRepository _repo;
        private readonly IMapper mapper;

        public Handler(IProductStageRepository repo, IMapper mapper)
        {
            _repo = repo;
            this.mapper = mapper;
        }

        public async Task<ProductStageModel.Out> Handle(Query request, CancellationToken cancellationToken)
        {
            Domain.Entities.ProductStage item =
                await _repo.GetOneAsync(request.Id) ??
                throw new NotFoundEntityException("This stage don't exist !");
            return mapper.Map<ProductStageModel.Out>(item);
        }
    }
}
