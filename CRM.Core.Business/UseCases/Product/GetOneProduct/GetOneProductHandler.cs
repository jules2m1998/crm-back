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

public class GetOneProductHandler : IRequestHandler<GetOneProductQuery, ProductOutModel?>
{
    private readonly IProductRepository _repo;

    public GetOneProductHandler(IProductRepository repo)
    {
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
    public async Task<ProductOutModel?> Handle(GetOneProductQuery request, CancellationToken cancellationToken) =>
        (await _repo.GetProductByIdAsync(request.Id))?
        .ToProductOutModel();
}
