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

namespace CRM.Core.Business.UseCases.ContactsUCs.GetOneContact;

public class GetOneContactHandler : IRequestHandler<GetOneContactQuery, ContactOutModel>
{
    private readonly IUserRepository _userRepo;
    private readonly IContactRepository _repo;

    public GetOneContactHandler(IUserRepository userRepo, IContactRepository repo)
    {
        _userRepo = userRepo;
        _repo = repo;
    }

    public async Task<ContactOutModel> Handle(GetOneContactQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);
        Contact? contact = (isAdmin
            ? await _repo.GetOneAsync(id: request.id, cancellationToken)
            : await _repo.GetOneAsync(id: request.id, user.Id, cancellationToken)) ?? throw new NotFoundEntityException("This contact doesn't exist !");

        return contact.ToModel();
    }
}
