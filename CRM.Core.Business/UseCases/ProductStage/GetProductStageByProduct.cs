using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProductStage;

public class GetProductStageByProduct
{
    public record Query(Guid ProductId): IRequest<IEnumerable<ProductStageModel.Out>>;
    public class Handler : IRequestHandler<Query, IEnumerable<ProductStageModel.Out>>
    {
        private readonly IProductStageRepository _repo;
        private readonly IMapper _mapper;

        public Handler(IProductStageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductStageModel.Out>> Handle(Query request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.ProductStage> result = await _repo.GetByProductAsync(request.ProductId);
            return _mapper.Map<IEnumerable<ProductStageModel.Out>>(result);
        }
    }
}
