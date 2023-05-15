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
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        var product = await _productRepo.GetProductByIdAsync(request.Model.ProductId) ?? throw new NotFoundEntityException("Product not found");
        var company = await _companyRepo.GetOneAsync(request.Model.CompanyId) ?? throw new NotFoundEntityException("Company not found");
        var supervisionHistory = await _supervisorHistoryRepo.GetUserSupervisor(request.Model.AgentId);
        var agent = supervisionHistory?.Supervised;

        if (
            !isAdmin &&
            (supervisionHistory != null
            &&
            (!supervisionHistory.IsActive || supervisionHistory.Supervisor.UserName != request.UserName))
            ) throw new UnauthorizedAccessException();

        Prospect? current = await _repo.GetTheCurrentAsync(request.Model.ProductId, request.Model.CompanyId);
        var errors = new Dictionary<string, List<string>>();
        if (current is not null && current.Agent.Id == request.Model.AgentId)
            errors.Add("AgentId", new List<string>() { "This assignation already exist !" });
        if (current is not null)
            errors.Add("ProductId", new List<string>() { "this company is already prospected for this product !" });

        if (current is not null) throw new BaseException(errors);

        if (agent == null && isAdmin)
        {
            agent = await _userRepo.GetUserByRoleAsync(userId: request.Model.AgentId, role: Roles.CCL);
        }
        if(agent is null) throw new NotFoundEntityException("Agent not found");

        var prospect = new Prospect(product, company, agent, user);
        Prospect p = await _repo.SaveAsync(prospect);

        return p.ToModel();
    }
}
