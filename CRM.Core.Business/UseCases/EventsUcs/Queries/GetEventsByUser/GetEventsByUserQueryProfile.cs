using AutoMapper;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventsByUser;

public class GetEventsByUserQueryProfile : Profile
{
    public GetEventsByUserQueryProfile()
    {
        CreateMap<Event, GetEventsByUserQueryDto>();
    }
}
