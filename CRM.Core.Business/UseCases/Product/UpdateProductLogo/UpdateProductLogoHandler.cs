using CRM.Core.Business.Extensions;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.UpdateProductLogo;

public class UpdateProductLogoHandler : IRequestHandler<UpdateProductLogoCommand, ProductOutModel>
{
    private readonly IUserRepository _userRepo;
    private readonly IProductRepository _repo;
    private readonly IFileHelper _fileHelper;

    public UpdateProductLogoHandler(IUserRepository userRepo, IProductRepository repo, IFileHelper fileHelper)
    {
        _userRepo = userRepo;
        _repo = repo;
        _fileHelper = fileHelper;
    }

    public async Task<ProductOutModel> Handle(UpdateProductLogoCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if(user is null) throw new UnauthorizedAccessException();

        var product = await _repo.GetProductByIdAsync(request.Id);
        if(product is null) throw new NotFoundEntityException("Product not found !");

        var isAdmin = _userRepo.IsAdminUser(user);
        var logos = await _fileHelper.SaveImageToServerAsync(request.Logo, new[] { "img", "products", "logo", "replace" });
        _fileHelper.DeleteImageToServer(product.Logo);
        product.Logo = logos.Item1;
        Domain.Entities.Product result = await _repo.UpdateOneAsync(product);

        return result.ToProductOutModel();
    }
}
