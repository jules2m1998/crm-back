using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.StageResponseUCs;

public static class GetAllByHead
{
    public record Query(Guid AgentId, Guid ProductId, Guid CompanyId) : IRequest<IEnumerable<StageResponseModel.Out>>;
    public class Handler : IRequestHandler<Query, IEnumerable<StageResponseModel.Out>>
    {
        private readonly IAsyncRepository<HeadProspection> _headRepo;
        private readonly IAsyncRepository<Commit> _commitRepo;
        private readonly IAsyncRepository<StageResponse> _responseRepo;
        private readonly IAsyncRepository<Domain.Entities.ProductStage> _productStageRepo;
        private readonly IMapper _mapper;

        public Handler(
            IAsyncRepository<HeadProspection> headRepo,
            IAsyncRepository<Commit> commitRepo,
            IAsyncRepository<StageResponse> responseRepo,
            IAsyncRepository<Domain.Entities.ProductStage> productStageRepo,
            IMapper mapper)
        {
            _headRepo = headRepo;
            _commitRepo = commitRepo;
            _responseRepo = responseRepo;
            _productStageRepo = productStageRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StageResponseModel.Out>> Handle(Query request, CancellationToken cancellationToken)
        {
            var head = await _headRepo.FindOneWhere(x => x.ProductId == request.ProductId && x.AgentId == request.AgentId && x.CompanyId == request.CompanyId) ?? throw new NotFoundEntityException("This prospection doesn't exist !");
            var commit = await _commitRepo.GetByIdAsync(head.CommitId) ?? throw new BaseException(new Dictionary<string, List<string>> { { nameof(HeadProspection.CommitId), new List<string> { "This commit doesn't exist !" } } });
            Domain.Entities.ProductStage? nextStage = (await _productStageRepo.FindManyWhere(x => x.ProductId == request.ProductId)).OrderBy(x => x.StageLevel).FirstOrDefault();
            var commitResponseId = commit.ResponseId;
            if(commitResponseId is not null)
            {
                var response = await _responseRepo.GetByIdAsync((Guid)commitResponseId) ?? throw new BaseException(new Dictionary<string, List<string>> { { nameof(commit.ResponseId), new List<string> { "This response doesn't exist !" } } });
                if(response.NextStageId is not null)
                {
                    nextStage = await _productStageRepo.GetByIdAsync((Guid)response.NextStageId);
                }
            }
            if (nextStage is null) return Enumerable.Empty<StageResponseModel.Out>();

            var responses = await _responseRepo.FindManyWhere(x => x.StageId == nextStage.Id);
            return _mapper.Map<IReadOnlyList<StageResponseModel.Out>>(responses);
        }
    }
}
