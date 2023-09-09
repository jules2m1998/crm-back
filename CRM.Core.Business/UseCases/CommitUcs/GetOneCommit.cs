using AutoMapper;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CommitUcs;

public static class GetOneCommit
{
    public record Query(Guid Id): IRequest<AddCommit.CommitOuModel>;
    public class Handler : IRequestHandler<Query, AddCommit.CommitOuModel>
    {
        private readonly IMapper mapper;
        private readonly ICommitRepository repo;

        public Handler(IMapper mapper, ICommitRepository repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public async Task<AddCommit.CommitOuModel> Handle(Query request, CancellationToken cancellationToken)
        {
            Commit commit = await repo.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundEntityException("This commit doesn't exist !");
            return mapper.Map<AddCommit.CommitOuModel>(commit);
        }
    }
}
