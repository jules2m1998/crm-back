using AutoMapper;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CommitUcs;

public static class UpdateCommit
{
    public record Command(AddCommit.AddCommitModel Model, Guid CommiId): IRequest<AddCommit.CommitOuModel>;
    public class Handler : IRequestHandler<Command, AddCommit.CommitOuModel>
    {
        private readonly IMapper mapper;
        private readonly ICommitRepository repo;

        public Handler(IMapper mapper, ICommitRepository repo)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        public async Task<AddCommit.CommitOuModel> Handle(Command request, CancellationToken cancellationToken)
        {
            var commit = await repo.GetByIdAsync(request.CommiId, cancellationToken) ?? throw new NotFoundEntityException("This commit doesn't exist !");
            var model = request.Model;
            commit.Message = model.Message;
            commit.ParentId = model.ParentId;
            commit.ResponseId = model.ResponseId;
            await repo.UpdateAsync(commit, cancellationToken);
            return mapper.Map<AddCommit.CommitOuModel>(commit);
        }
    }
}
