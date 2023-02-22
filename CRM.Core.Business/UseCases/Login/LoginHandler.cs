using CRM.Core.Business.Authentication;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, UserModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTService _jwtService;

        public LoginHandler(IUserRepository userRepository, IJWTService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<UserModel> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            ValidatorBehavior<LoginQuery>.Validate(request);
            var userRoles = await _userRepository.GetByUserAndRoleAsync(request.UserName, request.Password);
            if (userRoles == null || !userRoles.Item1.IsActivated) throw new UnauthorizedAccessException();
            var user = userRoles.Item1;
            var roles = userRoles.Item2;
            var token = _jwtService.Generate(user, roles);

            return new UserModel(
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                roles.Select(r => r.Name).ToList<string>(),
                user.Picture, 
                user.PhoneNumber, 
                user.CreatedAt, 
                user.UpdateAt, 
                user.DeletedAt, 
                token);
        }
    }
}
