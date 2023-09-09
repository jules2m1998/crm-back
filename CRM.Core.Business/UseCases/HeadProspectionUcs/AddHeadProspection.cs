using AutoMapper;
using CRM.Core.Business.Models.HeadProspectionModel;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.HeadProspectionUcs;

public static class AddHeadProspection
{
    public record Command(HeadProspectionInModel Model, string Username): IRequest<HeadProspectionOuModel>;
    public class Handler : IRequestHandler<Command, HeadProspectionOuModel>
    {
        private readonly IMapper mapper;
        private readonly IHeadProspectionRepository repo;
        private readonly IUserRepository userRepo;
        private readonly ICommitRepository commitRepo;

        public Handler(IMapper mapper, IHeadProspectionRepository repo, IUserRepository userRepo, ICommitRepository commitRepo)
        {
            this.repo = repo;
            this.userRepo = userRepo;
            this.commitRepo = commitRepo;
            this.mapper = mapper;
            this.repo = repo;
        }

        public async Task<HeadProspectionOuModel> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetUserAndRolesAsync(request.Username) ?? throw new UnauthorizedAccessException();
            var commit = new Commit
            {
                Creator = user
            };
            await commitRepo.AddAsync(commit, cancellationToken);
            var modal = request.Model;
            HeadProspection? old = await repo.GetByIndexAsync(modal.ProductId, modal.CompanyId, modal.AgentId, cancellationToken);
            if (old != null) throw new DuplicateNameException();
            var head = mapper.Map<HeadProspection>(request.Model);
            head.Creator = user.Creator;
            head.Commit = commit;
            _ = await repo.AddAdync(head, cancellationToken);
            return mapper.Map<HeadProspectionOuModel>(head);
        }
    }
}
