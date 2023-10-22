using AutoMapper;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdQueryResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IHeadProspectionRepository _headProspectionRepository;
    private readonly IMapper _mapper;

    public GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper, IHeadProspectionRepository headProspectionRepository)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _headProspectionRepository = headProspectionRepository;
    }

    public async Task<GetEventByIdQueryResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetEventWithHeadAndContactByIdAsync(request.EventId);
        if (@event == null) return new GetEventByIdQueryResponse
        {
            Success = false,
            Message = "This event doen't exist"
        };

        return new GetEventByIdQueryResponse
        {
            Success = true,
            Message = "Event found successfully",
            Data = _mapper.Map<GetEventByIdQueryDto>(@event)
        };
    }
}
