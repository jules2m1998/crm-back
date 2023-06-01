using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Event;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;

namespace CRM.Core.Business.UseCases.Events;

public static class GetEvents
{
    public record Query(string UserName): IRequest<ICollection<EventOutModel>>;

    public class Handler : IRequestHandler<Query, ICollection<EventOutModel>>
    {
        private readonly IEventRepository _repo;
        private readonly IUserRepository _userRepo;

        public Handler(IEventRepository repo, IUserRepository userRepository)
        {
            _repo = repo;
            _userRepo = userRepository;
        }

        public async Task<ICollection<EventOutModel>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
            var isAdmin = _userRepo.IsAdminUser(user);

            ICollection<Event> events = 
                isAdmin 
                ? await _repo.GetAsync() 
                : await _repo.GetByUserAsync(request.UserName);

            return events
                .Select(e => e.ToSimpleModel())
                .ToList();
        }
    }
}
