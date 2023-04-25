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

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetAllProspection;

public class GetAllProspectionHandler : IRequestHandler<GetAllProspectionQuery, ICollection<ProspectionOutModel>>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;

    public GetAllProspectionHandler(IProspectionRepository repository, IUserRepository userRepository)
    {
        _repo = repository;
        _userRepo = userRepository;
    }

    public async Task<ICollection<ProspectionOutModel>> Handle(GetAllProspectionQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();

        if (_userRepo.IsAdminUser(user)) return Convert(await _repo.GetAllAsync());
        else return Convert(await _repo.GetAllAsync(request.UserName));
    }

    private static ICollection<ProspectionOutModel> Convert(ICollection<Prospect> prospects)
        => prospects.Select(p => p.ToModel()).ToList();
        
}
