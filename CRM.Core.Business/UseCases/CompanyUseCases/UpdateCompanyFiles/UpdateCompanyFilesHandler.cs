using CRM.Core.Business.Extensions;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.UpdateCompanyFiles;

public class UpdateCompanyFilesHandler : IRequestHandler<UpdateCompanyFilesCommand, CompanyOutModel>
{
    private readonly IUserRepository _userRepo;
    private readonly ICompanyRepository _repo;
    private readonly IFileHelper _fileHelper;

    public UpdateCompanyFilesHandler(IUserRepository userRepo, ICompanyRepository repo, IFileHelper fileHelper)
    {
        _userRepo = userRepo;
        _repo = repo;
        _fileHelper = fileHelper;
    }

    public async Task<CompanyOutModel> Handle(UpdateCompanyFilesCommand request, CancellationToken cancellationToken)
    {
        var ceoPic = request.CompanyFiles.CEOPicture;
        var logo = request.CompanyFiles.Logo;
        if (ceoPic is null && logo is null) throw new BaseException(new Dictionary<string, List<string>> { { "request", new List<string> { "Invalid two file null or empty" } } });
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user is null) throw new UnauthorizedAccessException();

        var company = _userRepo.IsAdminUser(user) ? await _repo.GetOneAsync(request.Id) : await _repo.GetOneAsync(request.Id, request.UserName);
        if (company is null) throw new NotFoundEntityException(null);

        if(ceoPic is not null)
        {
            var nPics = await _fileHelper.SaveImageToServerAsync(ceoPic, new[] { "img", "company", "CEO" });
            _fileHelper.DeleteImageToServer(company.CEOPicture);
            company.CEOPicture = nPics.Item1;
        }
        if(logo is not null)
        {
            var nPics = await _fileHelper.SaveImageToServerAsync(logo, new[] { "img", "company", "logo" });
            _fileHelper.DeleteImageToServer(company.Logo);
            company.Logo = nPics.Item1;
        }
        company.UpdateAt = DateTime.UtcNow;

        var result = await _repo.UpdateAsync(company);
        return result.ToCompanyOutModel();
    }
}
