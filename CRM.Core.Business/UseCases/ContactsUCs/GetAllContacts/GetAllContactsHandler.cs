using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;

namespace CRM.Core.Business.UseCases.ContactsUCs.GetAllContacts;

public class GetAllContactsHandler : IRequestHandler<GetAllContactsQuery, ICollection<ContactOutModel>>
{
    private readonly IContactRepository _repo;
    private readonly IUserRepository _userRepo;

    public GetAllContactsHandler(IContactRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<ICollection<ContactOutModel>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        User current = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(current);
        ICollection<Contact> contacts = isAdmin ? await _repo.GetAsync() : await _repo.GetMineAsync(userId: current.Id);

        return contacts.Select(c => c.ToModel()).ToList();
    }
}
