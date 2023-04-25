using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.GetAllProduct;

public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, ICollection<ProductOutModel>>
{
    private readonly IProductRepository _repository;
    private readonly IUserRepository _userRepository;
    public GetAllProductHandler(IProductRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<ICollection<ProductOutModel>> Handle(GetAllProductQuery request, CancellationToken cancellationToken) =>
        (await _repository.GetAllAsync())
        .Select(p => p.ToProductOutModel())
        .ToList();
}
