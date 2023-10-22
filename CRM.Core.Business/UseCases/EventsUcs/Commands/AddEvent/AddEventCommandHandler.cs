using AutoMapper;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.Services;
using CRM.Core.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.AddEvent;

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, AddEventCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly IHeadProspectionRepository _headProspectionRepo;
    private readonly IEmailRepository _emailRepo;
    private readonly IContactRepository _contactRepo;
    private readonly IHttpContextService _httpContextService;
    private readonly IEventRepository _eventRepo;
    private readonly ILogger<AddEventCommandHandler> _logger;

    public AddEventCommandHandler(IMapper mapper, IHeadProspectionRepository headProspectionRepo, IEmailRepository emailRepo, IContactRepository contactRepo, IHttpContextService httpContextService, IEventRepository eventRepo, ILogger<AddEventCommandHandler> logger)
    {
        _mapper = mapper;
        _headProspectionRepo = headProspectionRepo;
        _emailRepo = emailRepo;
        _contactRepo = contactRepo;
        _httpContextService = httpContextService;
        _eventRepo = eventRepo;
        _logger = logger;
    }

    public async Task<AddEventCommandResponse> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        var validator = new AddEventCommandValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new AddEventCommandResponse
            {
                Success = false,
                Message = "Some fields are not valid",
                Errors = validationResult
                .Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray())
            };
        }
        var @event = _mapper.Map<Event>(request);
        var contacts = await _contactRepo.GetContactByUserAsync(request.ContactIds, _httpContextService.GetConnectedUserName() ?? string.Empty);

        try
        {
            await _eventRepo.AddAsync(@event);
            @event.Contact = contacts;
            await _eventRepo.UpdateAsync(@event);

            await _emailRepo.AddAsync(@event);

            var dto = _mapper.Map<AddEventCommandDto>(@event);
            return new AddEventCommandResponse
            {
                Data = dto,
                Success = true,
                Message = "Event successfully saved!"
            };
        }catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message, @event);
            return new AddEventCommandResponse
            {
                Success = false,
                Message = "Some fields are not valid verify your prospection or contacts",
                Errors = null
            };
        }
    }
}
