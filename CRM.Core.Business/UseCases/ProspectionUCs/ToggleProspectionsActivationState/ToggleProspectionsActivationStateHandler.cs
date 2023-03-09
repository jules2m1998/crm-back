using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;

namespace CRM.Core.Business.UseCases.ProspectionUCs.ToggleProspectionsActivationState;

public class ToggleProspectionsActivationStateHandler : IRequestHandler<ToggleProspectionsActivationStateCommand, ICollection<ProspectionOutModel>>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;

    public ToggleProspectionsActivationStateHandler(IProspectionRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<ICollection<ProspectionOutModel>> Handle(ToggleProspectionsActivationStateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        ICollection<Prospect> prospections =
            isAdmin
            ? await _repo.GetManyAsync(request.Models)
            : await _repo.GetManyAsync(request.Models, request.UserName);

        if (prospections.Count != request.Models.Count) throw new NotFoundEntityException("One or more prospection are not found !");
        foreach(Prospect prospection in prospections )
        {
            prospection.IsActivated = !prospection.IsActivated;
        }
        ICollection<Prospect> prospectionsEdited = await _repo.UpdateAsync(prospections);

        return prospectionsEdited.Select(p => p.ToModel()).ToList();
    }
}
