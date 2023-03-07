using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.DeleteManyProduct;

public class DeleteManyProductsHandler : IRequestHandler<DeleteManyProductsCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly IProductRepository _productRepo;

    public DeleteManyProductsHandler(IUserRepository userRepo, IProductRepository productRepo)
    {
        _userRepo = userRepo;
        _productRepo = productRepo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public async Task<bool> Handle(DeleteManyProductsCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user is null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        ICollection<Domain.Entities.Product> products = isAdmin ? await _productRepo.GetManyByIdsAsync(request.Ids): await _productRepo.GetManyByIdsAsync(request.Ids, request.UserName);
        if (request.Ids.Count != products.Count) return false;
        try
        {
            if(isAdmin)
            {
                await _productRepo.DeleteManyAsync(products);
            }
            else
            {
                await _productRepo.MarkAsDeletedAsync(products);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
