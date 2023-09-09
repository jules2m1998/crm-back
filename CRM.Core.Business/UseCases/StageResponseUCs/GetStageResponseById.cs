using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.StageResponseUCs;

public static class GetStageResponseById
{
    public record Query(Guid Id): IRequest<StageResponseModel.Out?>;
    public class Handler : IRequestHandler<Query, StageResponseModel.Out?>
    {
        private readonly IStageResponseRepository repo;
        private readonly IMapper mapper;

        public Handler(IStageResponseRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<StageResponseModel.Out?> Handle(Query request, CancellationToken cancellationToken)
        {
            StageResponse? item = await repo.GetByIdAsync(request.Id);
            if (item != null) return mapper.Map<StageResponseModel.Out>(item);
            return null;

        }
    }
}
