using CRM.core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.core.Queries
{
    public record GetPersonListQuery(): IRequest<List<Person>>;
}
