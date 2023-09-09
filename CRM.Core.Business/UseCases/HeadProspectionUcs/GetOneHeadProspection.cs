using AutoMapper;
using CRM.Core.Business.Models.HeadProspectionModel;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.HeadProspectionUcs;

public static class GetOneHeadProspection
{
    public record Query(Guid ProductId, Guid CompanyId, Guid AgentId): IRequest<HeadProspectionOuModel>;
    public class Handler : IRequestHandler<Query, HeadProspectionOuModel>
    {
        private readonly IMapper mapper;
        private readonly IHeadProspectionRepository repo;

        public Handler(IMapper mapper, IHeadProspectionRepository repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public async Task<HeadProspectionOuModel> Handle(Query request, CancellationToken cancellationToken)
        {

            HeadProspection data = await repo.GetByIndexAsync(request.ProductId, request.CompanyId, request.AgentId, cancellationToken) ?? throw new NotFoundEntityException("This prospection doesn't exist !");
            return mapper.Map<HeadProspectionOuModel>(data);
        }
    }
}
