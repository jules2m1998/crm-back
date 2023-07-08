using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProductStage;

public static class AddProductStage
{
    public record Command(ProductStageModel.In Model, string UserName): IRequest<ProductStageModel.Out>;

    public class Handler : IRequestHandler<Command, ProductStageModel.Out>
    {
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;
        private readonly IProductStageRepository _repo;

        public Handler(IProductRepository productRepo, IUserRepository userRepo, IProductStageRepository repo)
        {
            _productRepo = productRepo;
            _userRepo = userRepo;
            _repo = repo;
        }

        public async Task<ProductStageModel.Out> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
            if (user == null || user.UserRoles.FirstOrDefault(u => u.Role.Name == Roles.ADMIN) == null)
                throw new UnauthorizedAccessException();

            var model = request.Model;
            Domain.Entities.Product product = await _productRepo.GetWithStageAsync(model.ProductId) ?? throw new NotFoundEntityException("This product doesn't exist !");
            if (product.FirstStage != null) throw new BaseException(new Dictionary<string, List<string>>() { { "ProductId", new List<string>() { "This product already has a fir stage entry !"} } });
            var stage = new Domain.Entities.ProductStage
            {
                Name = model.Name,
                IsFirst = model.IsFirst,
                Creator = user
            };
            await _repo.CreateAsync(stage);
            product.FirstStage = stage;
            await _productRepo.UpdateOneAsync(product);
            return stage.ToModel();
        }
    }
}
