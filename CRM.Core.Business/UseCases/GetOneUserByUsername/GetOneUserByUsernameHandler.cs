using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.GetOneUserByUsername
{
    public class GetOneUserByUsernameHandler : IRequestHandler<GetOneUserByUsernameQuery, UserModel?>
    {
        private readonly IUserRepository _userRepository;

        public GetOneUserByUsernameHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserModel?> Handle(GetOneUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var isActive = await _userRepository.IsActivatedUserAsync(request.UserName);
            if (!isActive) throw new UnauthorizedAccessException();
            var userRole = await _userRepository.GetUserAndRole(request.UserName);
            if (userRole == null) return null;
            var user = userRole.Item1;
            var roles = userRole.Item2;
            return _userRepository.UserToUserModel(user, roles);
        }
    }
}
