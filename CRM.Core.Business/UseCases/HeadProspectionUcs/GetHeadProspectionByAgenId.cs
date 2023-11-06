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
public static class GetHeadProspectionByAgent
{
    public record Query(Guid AgentId) : IRequest<ICollection<HeadProspectionOuModel>>;
    public class Handler : IRequestHandler<Query, ICollection<HeadProspectionOuModel>>
    {
        private readonly IMapper mapper;    
        private readonly IHeadProspectionRepository repo;

        public Handler(IMapper mapper, IHeadProspectionRepository repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public async Task<ICollection<HeadProspectionOuModel>> Handle(Query request, CancellationToken cancellationToken)
        {

            ICollection<HeadProspection> data = await repo.GetByAgentIdAsync(request.AgentId, cancellationToken) ?? throw new NotFoundEntityException("This prospection doesn't exist !");
            return mapper.Map<ICollection<HeadProspectionOuModel>>(data);
        }
    }
}
