using AutoMapper;
using CRM.Core.Business.Extensions;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.Services;
using CRM.Core.Business.UseCases.EventsUcs.Commands.AddEvent;
using CRM.Core.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, UpdateEventCommandResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateEventCommandHandler> _logger;
    private readonly IContactRepository _contactRepo;
    private readonly IHttpContextService _httpContextService;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IMapper mapper, ILogger<UpdateEventCommandHandler> logger, IContactRepository contactRepo, IHttpContextService httpContextService)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _logger = logger;
        _contactRepo = contactRepo;
        _httpContextService = httpContextService;
    }

    public async Task<UpdateEventCommandResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateEventCommandValidator();
        var validatorResult = validator.Validate(request);
        if (!validatorResult.IsValid) return new UpdateEventCommandResponse
        {
            Success = false,
            Message = "Some fields are invalid",
            Errors = validatorResult.ToCustomDictionnary()
        };

        var @event = await _eventRepository.GetByIdAsync(request.EventId);
        if (@event == null) return new UpdateEventCommandResponse
        {
            Success = false,
            Message = "This event doesn't exist."
        };
        _mapper.Map(request, @event, typeof(UpdateEventCommand), typeof(Event));
        var contacts = await _contactRepo.GetContactByUserAsync(request.ContactIds, _httpContextService.GetConnectedUserName() ?? string.Empty);
        @event.Contact = contacts;
        try
        {
            await _eventRepository.UpdateAsync(@event);
            var newEvent = await _eventRepository.GetEventWithHeadAndContactByIdAsync(request.EventId);
            return new UpdateEventCommandResponse
            {
                Success = true,
                Message = "Event successfully updated.",
                Data = _mapper.Map<UpdateEventCommandDto>(newEvent)
            };
        }catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, @event);
            return new UpdateEventCommandResponse
            {
                Success = false,
                Message = "Some fields are not valid verify your prospection or contacts",
                Errors = null
            };
        }
    }
}
