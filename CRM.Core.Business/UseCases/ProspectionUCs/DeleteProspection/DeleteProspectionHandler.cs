using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;

namespace CRM.Core.Business.UseCases.ProspectionUCs.DeleteProspection;

public class DeleteProspectionHandler : IRequestHandler<DeleteProspectionCommand, ProspectionOutModel>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;

    public DeleteProspectionHandler(IProspectionRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<ProspectionOutModel> Handle(DeleteProspectionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();

        Prospect prospection =
            await _repo.GetOneAsync(request.AgentId, request.ProductId, request.CompanyId) 
            ?? throw new NotFoundEntityException("This propection does not exist !");
        if (user.Id != prospection.Agent.Id && !_userRepo.IsAdminUser(user)) throw new UnauthorizedAccessException();

        prospection.DeletedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(prospection);

        return prospection.ToModel();
    }
}
