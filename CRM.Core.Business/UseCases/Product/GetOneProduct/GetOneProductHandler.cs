using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.GetOneProduct;

public class GetOneProductHandler : IRequestHandler<GetOneProductQuery, ProductOutModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _repo;

    public GetOneProductHandler(IUserRepository userRepository, IProductRepository repo)
    {
        _userRepository = userRepository;
        _repo = repo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="NotFoundEntityException"></exception>
    public async Task<ProductOutModel> Handle(GetOneProductQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user is null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);
        Domain.Entities.Product? product = null!;
        if (isAdmin) product = await _repo.GetProductByIdAsync(request.Id);
        else product = await _repo.GetProductByIdAndCreatorAsync(request.Id, request.UserName);
        if (product is null) throw new NotFoundEntityException("This product not found !");

        return product.ToProductOutModel();
    }
}
