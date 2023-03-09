using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetProspectionByProduct;

internal class GetProspectionByProductHandler : IRequestHandler<GetProspectionByProductQuery, ICollection<ProspectionOutModel>>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;

    public GetProspectionByProductHandler(IProspectionRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<ICollection<ProspectionOutModel>> Handle(GetProspectionByProductQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        ICollection<Prospect> prospection =
            isAdmin
            ? await _repo.GetProspectionByProduct(request.ProductId)
            : await _repo.GetProspectionByProduct(request.ProductId, request.UserName);

        return prospection.Select(pr => pr.ToModel()).ToList();
    }
}
