using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.MarkAsDeletedRange
{
    public class MarkAsDeletedRangeHandler : IRequestHandler<MarkAsDeletedRangeQuery, bool>
    {
        private readonly IUserRepository _repo;

        public MarkAsDeletedRangeHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(MarkAsDeletedRangeQuery request, CancellationToken cancellationToken)
        {
            if (!request.Ids.Any() || string.IsNullOrEmpty(request.UserName)) return false;
            await _repo.MarkAsDeletedRangeAsync(request.Ids, request.UserName);
            return true;
            throw new NotImplementedException();
        }
    }
}
