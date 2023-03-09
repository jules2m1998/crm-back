using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Prospect;
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

namespace CRM.Core.Business.UseCases.ProspectionUCs.AttributeProspection;

public class AttributeProspectionHandler : IRequestHandler<AttributeProspectionCommand, ProspectionOutModel>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;
    private readonly IProductRepository _productRepo;
    private readonly ICompanyRepository _companyRepo;
    private readonly ISupervisionHistoryRepository _supervisorHistoryRepo;

    public AttributeProspectionHandler(
        IProspectionRepository repo, 
        IUserRepository userRepo, 
        IProductRepository productRepo, 
        ICompanyRepository companyRepo,
        ISupervisionHistoryRepository supervisorHistoryRepo
        )
    {
        _repo = repo;
        _userRepo = userRepo;
        _productRepo = productRepo;
        _companyRepo = companyRepo;
        _supervisorHistoryRepo = supervisorHistoryRepo;
    }

    public async Task<ProspectionOutModel> Handle(AttributeProspectionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        var product = await _productRepo.GetProductByIdAsync(request.Model.ProductId);
        if (product == null) throw new NotFoundEntityException("Product not found");

        var company = await _companyRepo.GetOneAsync(request.Model.CompanyId);
        if (company == null) throw new NotFoundEntityException("Company not found");

        var supervisionHistory = await _supervisorHistoryRepo.GetUserSupervisor(request.Model.AgentId);
        var agent = supervisionHistory?.Supervised;

        if (
            !isAdmin &&
            (supervisionHistory != null
            &&
            (!supervisionHistory.IsActive || supervisionHistory.Supervisor.UserName != request.UserName))
            ) throw new UnauthorizedAccessException();

        Prospect? current = await _repo.GetTheCurrentAsync(request.Model.ProductId, request.Model.CompanyId);
        if (current is not null && current.Agent.Id == request.Model.AgentId) throw new BaseException(new Dictionary<string, List<string>>()
        {
            {"AgentId", new List<string>(){"This assignation already exist !"} }
        });

        if (agent == null && isAdmin)
        {
            agent = await _userRepo.GetUserByRoleAsync(userId: request.Model.AgentId, role: Roles.CCL);
        }
        if(agent is null) throw new NotFoundEntityException("Agent not found");

        var prospect = new Prospect(product, company, agent, user);
        Prospect p = await _repo.SaveAsync(prospect);

        return p.ToModel();
        throw new NotImplementedException();
    }
}
