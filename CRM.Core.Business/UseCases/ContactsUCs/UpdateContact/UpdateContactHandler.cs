using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.ContactsUCs.DeleteContact;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.UpdateContact;

public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, ContactOutModel>
{
    private readonly IContactRepository _repo;
    private readonly IUserRepository _uRepo;
    private readonly ICompanyRepository _cRepo;
    private readonly IPhoneRepository _pRepo;

    public UpdateContactHandler(IContactRepository repo, IUserRepository uRepo, ICompanyRepository cRepo, IPhoneRepository pRepo)
    {
        _repo = repo;
        _uRepo = uRepo;
        _cRepo = cRepo;
        _pRepo = pRepo;
    }

    public async Task<ContactOutModel> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var user = await _uRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
        var isAdmin = _uRepo.IsAdminUser(user);
        var contact = (isAdmin
            ? await _repo.GetOneAsync(id: request.Id, cancellationToken)
            : await _repo.GetOneAsync(id: request.Id, user.Id, cancellationToken)) ?? throw new NotFoundEntityException("This contact doesn't exist !");
        Contact? existing = await _repo.GetAsync(request.Model.Name, request.Model.CompanyId, cancellationToken);
        if (existing != null) throw new BaseException(new Dictionary<string, List<string>> { { "Name", new List<string>() { "This contact already exist !" } }, { "CompanyId", new List<string>() { "This contact already exist !" } } });

        if (contact.Company.Id != request.Model.CompanyId)
        {
            var newCompany =
                await _cRepo.GetOneAsync(request.Model.CompanyId) 
                ?? throw new NotFoundEntityException("This company doesn't exist !");
            contact.Company = newCompany;
        }
        IEnumerable<User> shared = await _uRepo.GetUsers(request.Model.SharedTo ?? new List<Guid>());
        contact.SharedTo = shared.ToList();

        IEnumerable<PhoneNumber> phones = request.Model.Phones.Select(p => new PhoneNumber
        {
            Value = p
        });

        contact.Phones = phones.ToList();
        contact.Name = request.Model.Name;
        contact.Email = request.Model.Email;
        contact.Job = request.Model.Job;

        await _repo.UpdateAsync(contact);

        return contact.ToModel();
    }
}
