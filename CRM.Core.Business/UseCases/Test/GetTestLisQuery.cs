using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Test
{
    public record GetTestLisQuery(): IRequest<string[]>;
}
