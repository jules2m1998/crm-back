using AutoMapper;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CommitUcs;

public static class GetAllCommits
{
    public record Query(): IRequest<ICollection<AddCommit.CommitOuModel>>;
    public class Handler : IRequestHandler<Query, ICollection<AddCommit.CommitOuModel>>
    {
        private readonly IMapper mapper;
        private readonly ICommitRepository repo;

        public Handler(IMapper mapper, ICommitRepository repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public async Task<ICollection<AddCommit.CommitOuModel>> Handle(Query request, CancellationToken cancellationToken)
        {
            ICollection<Commit> commits = await repo.GetAllAsync(cancellationToken);
            return mapper.Map<ICollection<AddCommit.CommitOuModel>>(commits);
        }
    }
}
