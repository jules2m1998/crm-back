using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ContactsUCs.DeleteContact;

public class DeleteContactHandler : IRequestHandler<DeleteContactCommand>
{
    private readonly IContactRepository _repo;
    private readonly IUserRepository _userRepository;

    public DeleteContactHandler(IContactRepository repo, IUserRepository userRepository)
    {
        _repo = repo;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);
        var contact = (isAdmin
            ? await _repo.GetOneAsync(request.Id, cancellationToken)
            : await _repo.GetOneAsync(request.Id, user.Id, cancellationToken))
            ?? throw new NotFoundEntityException("This contact doesn't exist !");

        await _repo.RemoveAsync(contact);

        return new Unit();
    }

    Task IRequestHandler<DeleteContactCommand>.Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
