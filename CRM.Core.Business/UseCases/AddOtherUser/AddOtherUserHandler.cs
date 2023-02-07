using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using MediatR;
using System.Data;

namespace CRM.Core.Business.UseCases.AddOtherUser
{
    public class AddOtherUserHandler : IRequestHandler<AddOtherUserCommand, UserModel>
    {
        private readonly IUserRepository _repo;
        private readonly IFileHelper _fileHelper;
        public AddOtherUserHandler(IUserRepository userRepository, IFileHelper fileHelper)
        {
            _repo = userRepository;
            _fileHelper = fileHelper;
        }

        /// <summary>
        /// Add single user to database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<UserModel> Handle(AddOtherUserCommand request, CancellationToken cancellationToken)
        {
            if(request.User.Password == null) request.User.Password = DefaultParams.defaultPwd;
            var currentUserRoles = await _repo.GetUserAndRole(request.CurrentUserName);
            if (currentUserRoles == null) throw new UnauthorizedAccessException();

            var roles = currentUserRoles.Item2;
            if(roles == null) throw new UnauthorizedAccessException();
            List<string?> listRoles = roles.Select(v => v.Name).ToList();
            if (
                (request.User.Role == Roles.ADMIN) ||
                (listRoles.Contains(Roles.CLIENT)) ||
                request.User.Role == Roles.SUPERVISOR && listRoles.Contains(Roles.SUPERVISOR)
                ) throw new UnauthorizedAccessException();
            ValidatorBehavior<UserBodyAndRole>.Validate(request.User);

            var isClient = roles.Find(r => r.Name == Roles.CLIENT) != null;
            var isSupervisor = roles.Find(r => r.Name == Roles.SUPERVISOR) != null;
            var isCCL = roles.Find(r => r.Name == Roles.CCL) != null;
            if (isClient) throw new UnauthorizedAccessException();
            if (isSupervisor && (request.User.Role == Roles.ADMIN || request.User.Role == Roles.SUPERVISOR)) throw new UnauthorizedAccessException();
            if (isCCL && (request.User.Role == Roles.ADMIN || request.User.Role == Roles.SUPERVISOR || request.User.Role == Roles.CCL)) throw new UnauthorizedAccessException();
            var cur = request.User;
            string? picture = null;
            if (cur.Picture is not null)
            {
                var result = await _fileHelper.SaveImageToServerAsync(cur.Picture, new[] { cur.Role, "pictures" });
                picture = result.Item1;
            }

            var user = new User()
            {
                UserName = cur.UserName,
                Email = cur.Email,
                FirstName = cur.FirstName,
                LastName = cur.LastName,
                PhoneNumber = cur.PhoneNumber,
                Creator = currentUserRoles.Item1,
            };
            if (picture is not null) user.Picture = picture;
            var userAndRole = await _repo.AddAsync(user, request.User.Password ?? DefaultParams.defaultPwd, request.User.Role);
            return userAndRole;
        }
    }
}
