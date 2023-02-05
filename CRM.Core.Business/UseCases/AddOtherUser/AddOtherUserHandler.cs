using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using MediatR;

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

        public async Task<UserModel> Handle(AddOtherUserCommand request, CancellationToken cancellationToken)
        {
            request.User.Password = DefaultParams.defaultPwd;
            if (
                (request.User.Role == Roles.ADMIN) ||
                (request.User.Role == Roles.CLIIENT) ||
                request.User.Role == Roles.SUPERVISOR && request.Roles.Contains(Roles.SUPERVISOR)
                ) throw new UnauthorizedAccessException();
            ValidatorBehavior<UserBodyAndRole>.Validate(request.User);
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
                PhoneNumber = cur.PhoneNumber
            };
            if (picture is not null) user.Picture = picture;
            var userAndRole = await _repo.AddAsync(user, request.User.Password, request.User.Role);
            return userAndRole;
        }
    }
}
