using AutoMapper;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventById;

public class GetEventByIdQueryProfile : Profile
{
    public GetEventByIdQueryProfile()
    {
        CreateMap<Event, GetEventByIdQueryDto>();
    }
}
