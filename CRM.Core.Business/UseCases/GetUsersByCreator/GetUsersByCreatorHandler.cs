using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.GetUsersByCreator
{
    public class GetUsersByCreatorHandler : IRequestHandler<GetUsersByCreatorQuery, ICollection<UserAndCreatorModel>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersByCreatorHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ICollection<UserAndCreatorModel>> Handle(GetUsersByCreatorQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.CreatorUserName)) throw new UnauthorizedAccessException();
            return await _userRepository.GetUsersByCreatorUserNameAsync(request.CreatorUserName);
        }
    }
}
