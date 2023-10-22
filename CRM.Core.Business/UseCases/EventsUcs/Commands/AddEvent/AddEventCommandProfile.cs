using AutoMapper;
using CRM.Core.Domain.Entities;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.AddEvent;

public class AddEventCommandProfile : Profile
{
    public AddEventCommandProfile()
    {
        CreateMap<AddEventCommand, Event>();
        CreateMap<Event, AddEventCommandDto>();

        CreateMap<HeadProspection, AddEventCommandHeadProspectionDto>();
    }
}
