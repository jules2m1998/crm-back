using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;

namespace CRM.Core.Business.UseCases.CompanyUseCases.GetOneCompany;

public class GetOneCompanyHandler : IRequestHandler<GetOneCompanyCommand, CompanyOutModel>
{
    private readonly ICompanyRepository _repo;
    private readonly IUserRepository _userRepository;

    public GetOneCompanyHandler(ICompanyRepository repo, IUserRepository userRepository)
    {
        _repo = repo;
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
    public async Task<CompanyOutModel> Handle(GetOneCompanyCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);
        Company? company;
        if (isAdmin) company = await _repo.GetOneAsync(request.Id);
        else company = await _repo.GetOneAsync(request.Id, request.UserName);
        if (company == null) throw new NotFoundEntityException("Company not found !");
        return company.ToCompanyOutModel();
    }
}
