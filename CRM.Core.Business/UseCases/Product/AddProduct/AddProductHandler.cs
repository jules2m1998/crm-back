using CRM.Core.Business.Extensions;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.AddProduct;

public class AddProductHandler : IRequestHandler<AddProductCommand, ProductOutModel>
{
    private readonly IProductRepository _repo;
    private readonly IUserRepository _userRepository;
    private readonly IFileHelper _fileHelper;

    public AddProductHandler(IProductRepository product, IUserRepository userRepository, IFileHelper fileHelper)
    {
        _repo = product;
        _userRepository = userRepository;
        _fileHelper = fileHelper;
    }

    async Task<ProductOutModel> IRequestHandler<AddProductCommand, ProductOutModel>.Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null || user.UserRoles.FirstOrDefault(u => u.Role.Name == Roles.ADMIN) == null)
            throw new UnauthorizedAccessException();
        var productModel = request.Product;
        ValidatorBehavior<ProductInModel>.Validate(productModel);
        string logo = null!;
        Domain.Entities.Product? existing = await _repo.GetByNameAsync(productModel.Name);
        if (existing != null) throw new BaseException(new Dictionary<string, List<string>> { { "name", new List<string> { "This name is already taken !"} } });
        if(productModel.Logo is not null)
        {
            var fileSave = await _fileHelper.SaveImageToServerAsync(productModel.Logo, new[] { "img", "products", "logo" });
            if(fileSave != null)
            {
                logo = fileSave.Item1;
            }
        }
        else
        {
            logo = DefaultParams.defaultProduct;
        }
        var product = new Domain.Entities.Product(productModel.Name, logo, productModel.Description, user);
        Domain.Entities.Product result = await _repo.CreateOneAsync(product);
        return result.ToProductOutModel();
    }
}
