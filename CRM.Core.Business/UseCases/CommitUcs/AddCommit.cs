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

namespace CRM.Core.Business.UseCases.CommitUcs;

public static class AddCommit
{
    public class AddCommitModel
    {
        public string Message { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public Guid? ResponseId { get; set; }
    }

    public class CommitOuModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public StageResponseModel.Out? Response { get; set; }
        public CommitOuModel? Parent { get; set; }
    }

    public record Command(AddCommitModel Model, string Username): IRequest<CommitOuModel>;
    public class Handler : IRequestHandler<Command, CommitOuModel>
    {
        private readonly IMapper mapper;
        private readonly ICommitRepository repo;
        private readonly IUserRepository userRepo;

        public Handler(IMapper mapper, ICommitRepository repo, IUserRepository userRepo)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.userRepo = userRepo;
        }

        public async Task<CommitOuModel> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetUserAndRolesAsync(request.Username) ?? throw new UnauthorizedAccessException();
            var commit = mapper.Map<Commit>(request.Model);
            commit.Creator = user;
            await repo.AddAsync(commit, cancellationToken);

            return mapper.Map<CommitOuModel>(commit);
        }
    }
}
