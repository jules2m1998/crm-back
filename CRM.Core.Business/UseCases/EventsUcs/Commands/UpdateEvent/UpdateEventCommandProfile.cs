using AutoMapper;
using CRM.Core.Domain.Entities;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.UpdateEvent;

public class UpdateEventCommandProfile : Profile
{
    public UpdateEventCommandProfile()
    {
        CreateMap<UpdateEventCommand, Event>();
        CreateMap<Event, UpdateEventCommandDto>();
        CreateMap<User, UpdateEventCommandOwnerDto>();
        CreateMap<Contact, UpdateEventCommandContactDto>();
    }
}
