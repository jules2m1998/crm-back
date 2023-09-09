using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;

namespace CRM.Core.Business.UseCases.CommitUcs;

public static class DeleteCommit
{
    public record Command(Guid CommiId): IRequest<bool>;
    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly ICommitRepository repo;

        public Handler(ICommitRepository repo)
        {
            this.repo = repo;
        }
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var commit = await repo.GetByIdAsync(request.CommiId, cancellationToken) ?? throw new NotFoundEntityException("This commit doesn't exist !");
            int count = await repo.DeleteAsync(commit, cancellationToken);
            return count > 0;
        }
    }
}
