using CRM.Core.Business.Extensions;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.AddCompany;

public class AddCompanyHandler : IRequestHandler<AddCompanyCommand, CompanyOutModel>
{
    private readonly ICompanyRepository _repo;
    private readonly IUserRepository _userRepo;
    private readonly IFileHelper _fileHelper;

    public AddCompanyHandler(ICompanyRepository repo, IUserRepository userRepo, IFileHelper fileHelper)
    {
        _repo = repo;
        _userRepo = userRepo;
        _fileHelper = fileHelper;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public async Task<CompanyOutModel> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();

        var companyDto = request.Company;
        ValidatorBehavior<CompanyInModel>.Validate(companyDto);
        string logo = DefaultParams.defaultProduct;
        if(companyDto.Logo!= null)
        {
            var imgs = await _fileHelper.SaveImageToServerAsync(companyDto.Logo, new[] { "img", "company", "logo" });
            logo = imgs.Item1;
        }
        string ceoPic = DefaultParams.defaultCEOPicture;
        if(companyDto.CEOPicture is not null)
        {
            var imgs = await _fileHelper.SaveImageToServerAsync(companyDto.CEOPicture, new[] { "img", "company", "CEO" });
            ceoPic = imgs.Item1;
        }
        var company = new Company(companyDto.Name, companyDto.Description, logo, ceoPic, companyDto.CEOName, companyDto.Values, companyDto.Mission, companyDto.Concurrent, companyDto.Location, companyDto.ActivityArea, companyDto.Size, user);
        Company result = await _repo.AddOneAsync(company);
        return result.ToCompanyOutModel();
    }
}
