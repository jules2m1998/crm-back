using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using MediatR;

namespace CRM.Core.Business.UseCases.ProductStage;

public static class AddProductStage
{
    public record Command(IEnumerable<ProductStageModel.In> Model, string UserName): IRequest<IEnumerable<ProductStageModel.Out>>;

    public class Handler : IRequestHandler<Command, IEnumerable<ProductStageModel.Out>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IProductStageRepository _repo;
        private readonly IMapper mapper;

        public Handler(IUserRepository userRepo, IProductStageRepository repo, IMapper mapper)
        {
            _userRepo = userRepo;
            _repo = repo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductStageModel.Out>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
            if (user == null || user.UserRoles.FirstOrDefault(u => u.Role.Name == Roles.ADMIN) == null)
                throw new UnauthorizedAccessException();

            var stage = mapper.Map<IEnumerable<Domain.Entities.ProductStage>>(request.Model).Select(x =>
            {
                x.Creator = user;
                return x;
            });
            await _repo.AddRangeAsync(stage);
            return mapper.Map<IEnumerable<ProductStageModel.Out>>(stage);
        }
    }
}
