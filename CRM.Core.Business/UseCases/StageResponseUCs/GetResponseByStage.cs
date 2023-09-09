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

public static class GetResponseByStage
{
    public record Query(Guid QuestionId): IRequest<IEnumerable<StageResponseModel.Out>>;
    public class Handler : IRequestHandler<Query, IEnumerable<StageResponseModel.Out>>
    {
        private readonly IStageResponseRepository repo;
        private readonly IMapper mapper;

        public Handler(IStageResponseRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StageResponseModel.Out>> Handle(Query request, CancellationToken cancellationToken)
        {
            IEnumerable<StageResponse> responses = await repo.GetAllAsync(request.QuestionId, cancellationToken);
            return mapper.Map<IEnumerable<StageResponseModel.Out>>(responses);
        }
    }
}
