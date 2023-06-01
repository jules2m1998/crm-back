using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Events;

public static class DeleteEvent
{
    public record Command(Guid Id, string UserName): IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IEventRepository _repo;
        private readonly IUserRepository _userRepo;

        public Handler(IEventRepository repo, IUserRepository userRepository)
        {
            _repo = repo;
            _userRepo = userRepository;
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
            var isAdmin = _userRepo.IsAdminUser(user); 
            var e = (isAdmin ? await _repo.GetAsync(request.Id) : await _repo.GetAsync(request.Id, request.UserName)) ?? throw new NotFoundEntityException("Event not found !");

            await _repo.DeleteAsync(e);
            return Unit.Value;
        }
    }
}
