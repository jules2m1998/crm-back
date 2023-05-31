using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Event;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;

namespace CRM.Core.Business.UseCases.Events;

public static class GetOneEvents
{
    public record Query(Guid Id, string UserName): IRequest<EventOutModel?>;

    public class Handler : IRequestHandler<Query, EventOutModel?>
    {
        private readonly IEventRepository _repo;

        public Handler(IEventRepository repo)
        {
            _repo = repo;
        }

        public async Task<EventOutModel> Handle(Query request, CancellationToken cancellationToken)
        {
            var e = (await _repo.GetAsync(request.Id, request.UserName)) ?? throw new NotFoundEntityException("This event doesn't exist !");
            return e.ToModel();
        }
    }
}
