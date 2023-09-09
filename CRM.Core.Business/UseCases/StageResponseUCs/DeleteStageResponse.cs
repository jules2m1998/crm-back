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

public static class DeleteStageResponse
{
    public record Command(Guid Id): IRequest<StageResponseModel.Out?>;
    public class Handler : IRequestHandler<Command, StageResponseModel.Out?>
    {
        private readonly IStageResponseRepository repo;
        private readonly IMapper mapper;

        public Handler(IStageResponseRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<StageResponseModel.Out?> Handle(Command request, CancellationToken cancellationToken)
        {
            StageResponse? stageResponse = await repo.DeleteAsync(request.Id);
            if (stageResponse == null) return null;
            return mapper.Map<StageResponseModel.Out>(stageResponse);
        }
    }
}
