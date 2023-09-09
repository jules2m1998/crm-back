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

public static class DeleteHeadProspection
{
    public record Command(Guid ProductId, Guid CompanyId, Guid AgentId): IRequest<bool>;
    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly IHeadProspectionRepository repo;

        public Handler(IHeadProspectionRepository repo)
        {
            this.repo = repo;
        }
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            HeadProspection data = await repo.GetByIndexAsync(request.ProductId, request.CompanyId, request.AgentId, cancellationToken) ?? throw new NotFoundEntityException("This prospection doesn't exist !");
            int count = await repo.DeleteAsync(data, cancellationToken);
            return count > 0;
        }
    }
}
