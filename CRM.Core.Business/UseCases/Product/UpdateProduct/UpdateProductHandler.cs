using CRM.Core.Business.Extensions;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductOutModel>
{
    private readonly IProductRepository _repo;
    private readonly IFileHelper _fileHelper;
    private readonly IUserRepository _userRepository;

    public UpdateProductHandler(IProductRepository repo, IFileHelper fileHelper, IUserRepository userRepository)
    {
        _repo = repo;
        _fileHelper = fileHelper;
        _userRepository = userRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="NotFoundEntityException"></exception>
    public async Task<ProductOutModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if(user is null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);

        Domain.Entities.Product? pUpdate = null!;
        if (isAdmin) pUpdate = await _repo.GetProductByIdAsync(request.Id);
        else pUpdate = await _repo.GetProductByIdAndCreatorAsync(request.Id, user.UserName ?? "");
        if (pUpdate == null) throw new NotFoundEntityException("Product not exist !");
        Domain.Entities.Product u = await _repo.PathProductAsync(pathData: request.Product, product: pUpdate);
        return u.ToProductOutModel();
    }
}
