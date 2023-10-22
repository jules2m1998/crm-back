using AutoMapper;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.Services;
using CRM.Core.Domain.Entities;
using MediatR;

namespace CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventsByUser;

public class GetEventsByUserQueryHandler : IRequestHandler<GetEventsByUserQuery, GetEventsByUserQueryResponse>
{
    private readonly IHttpContextService _httpContextService;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventsByUserQueryHandler(IHttpContextService httpContextService, IEventRepository eventRepository, IMapper mapper)
    {
        _httpContextService = httpContextService;
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<GetEventsByUserQueryResponse> Handle(GetEventsByUserQuery request, CancellationToken cancellationToken)
    {
        ICollection<Event> events = await _eventRepository.GetEventsByOwnerAsync(_httpContextService.GetUserId(), cancellationToken);
        return new GetEventsByUserQueryResponse
        {
            Data = _mapper.Map<ICollection<GetEventsByUserQueryDto>>(events),
            Success = true,
            Message = "Event successfully found"
        };
    }
}
