using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using MediatR;

namespace CRM.Core.Business.UseCases.StageResponseUCs;

public static class AddStageResponse
{
    public record Command(StageResponseModel.In Model, string UserName): IRequest<StageResponseModel.Out>;
    public class Handler : IRequestHandler<Command, StageResponseModel.Out>
    {
        private readonly IMapper mapper;
        private readonly IStageResponseRepository repo;
        private readonly IUserRepository userRepo;

        public Handler(IMapper mapper, IStageResponseRepository repo, IUserRepository userRepo)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.userRepo = userRepo;
        }

        public async Task<StageResponseModel.Out> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetUserAndRolesAsync(request.UserName);
            if (user == null || user.UserRoles.FirstOrDefault(u => u.Role.Name == Roles.ADMIN) == null)
                throw new UnauthorizedAccessException();
            var response = mapper.Map<StageResponse>(request.Model);
            response.Creator = user;
            StageResponse result = await repo.AddAsync(response, cancellationToken);
            return mapper.Map<StageResponseModel.Out>(result);
        }
    }
}
