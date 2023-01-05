using CRM.core.DataAccess;
using CRM.core.Models;
using CRM.core.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.core.Handlers;

public class GetPersonListHandler : IRequestHandler<GetPersonListQuery, List<Person>>
{

    private readonly IDataAccess _dataAccess;
    public GetPersonListHandler(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<List<Person>> Handle(GetPersonListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dataAccess.GetPeople());
    }
}
