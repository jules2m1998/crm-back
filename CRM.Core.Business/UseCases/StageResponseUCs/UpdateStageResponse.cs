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

public static class UpdateStageResponse
{
    public record Command(Guid Id, StageResponseModel.In Model): IRequest<StageResponseModel.Out?>;
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
            var model = mapper.Map<StageResponse>(request.Model);
            StageResponse? edited = await repo.UpdateAsync(request.Id, model, cancellationToken);
            if (edited == null) return null;
            return mapper.Map<StageResponseModel.Out>(edited);
        }
    }
}
