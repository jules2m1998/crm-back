using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.AddContact;

public class AddContactHandler : IRequestHandler<AddContactCommand, ContactOutModel>
{
    private readonly IContactRepository _repo;
    private readonly ICompanyRepository _companyRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPhoneRepository _phoneRepo;

    public AddContactHandler(IContactRepository repo, ICompanyRepository companyRepo, IUserRepository userRepo, IPhoneRepository phoneRepo)
    {
        _repo = repo;
        _companyRepo = companyRepo;
        _userRepo = userRepo;
        _phoneRepo = phoneRepo;
    }

    public async Task<ContactOutModel> Handle(AddContactCommand request, CancellationToken cancellationToken)
    {
        Contact? existing = await _repo.GetAsync(request.Name, request.CompanyId, cancellationToken);
        if (existing != null) throw new BaseException(new Dictionary<string, List<string>> { { "Name", new List<string>() { "This contact already exist !"} }, { "CompanyId", new List<string>() { "This contact already exist is this company !" } } });

        var creator = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
        var company = await _companyRepo.GetOneAsync(request.CompanyId) ?? throw new NotFoundEntityException("Company not found !");
        ICollection<PhoneNumber> phones = await _phoneRepo.GetManyAsync(values: request.Phones);

        if(phones.Count > 0) 
            throw new BaseException(new Dictionary<string, List<string>> { { "Phones", phones.Select(p => p.Value).ToList() } });

        ICollection<User>? sharedUsers = null;
        if (request.SharedTo?.Count > 0)
        {
            sharedUsers =
            request.SharedTo is null or { Count: 0 }
            ? null
            : await _userRepo.GetUsers(ids: request.SharedTo);

            if (sharedUsers == null || request.SharedTo == null || sharedUsers.Count != request.SharedTo.Count) 
                throw new NotFoundEntityException("Some users for whom the contact was shared do not exist.");
        }

        var contact = new Contact
        {
            Name = request.Name,
            Email = request.Email,
            Job = request.Job,
            Phones = request.Phones.Select(p => new PhoneNumber { Value = p }).ToList(),
            Visibility = request.Visibility,
            Company = company,
            Creator = creator
        };

        if(sharedUsers?.Count > 0) contact.SharedTo = sharedUsers;
        await _repo.AddAsync(contact, cancellationToken);

        return contact.ToModel();
    }
}
