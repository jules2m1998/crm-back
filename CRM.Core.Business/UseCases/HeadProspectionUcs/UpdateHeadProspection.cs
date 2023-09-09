using AutoMapper;
using CRM.Core.Business.Models.HeadProspectionModel;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.HeadProspectionUcs;

public static class UpdateHeadProspection
{
    public record Command(Guid ProductId, Guid CompanyId, Guid AgentId, Guid commitId) : IRequest<HeadProspectionOuModel>;
    public class Handler : IRequestHandler<Command, HeadProspectionOuModel>
    {
        private readonly IHeadProspectionRepository repo;
        private readonly IMapper mapper;

        public Handler(IHeadProspectionRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<HeadProspectionOuModel> Handle(Command request, CancellationToken cancellationToken)
        {
            HeadProspection data = await repo.GetByIndexAsync(request.ProductId, request.CompanyId, request.AgentId, cancellationToken) ?? throw new NotFoundEntityException("This prospection doesn't exist !");
            data.CommitId = request.commitId;
            await repo.UpdateAsync(data, cancellationToken);
            return mapper.Map<HeadProspectionOuModel>(data);
        }
    }
}
