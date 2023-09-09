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
        private readonly IHeadProspectionRepository headRepo;
        private readonly IStageResponseRepository repo;
        private readonly IMapper mapper;

        public Handler(IHeadProspectionRepository headRepo, IStageResponseRepository repo, IMapper mapper)
        {
            this.headRepo = headRepo;
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StageResponseModel.Out>> Handle(Query request, CancellationToken cancellationToken)
        {
            var head = await headRepo.GetByIndexAsync(request.ProductId, request.CompanyId, request.AgentId, cancellationToken) ?? throw new NotFoundEntityException("This head doesn't exist !");
            IEnumerable<StageResponse> response = await repo.GetAllByStageResponseAsync(productId: head.Product.Id, responseId: head.Commit.Response?.Id, cancellationToken);
            return mapper.Map<IEnumerable<StageResponseModel.Out>>(response);
        }
    }
}
