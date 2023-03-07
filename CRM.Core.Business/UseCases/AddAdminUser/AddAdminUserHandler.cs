using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using MediatR;

namespace CRM.Core.Business.UseCases.AddUser;

public class AddAdminUserHandler : IRequestHandler<AddAdminUserCommand, UserModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IFileHelper _fileHelper;
    public AddAdminUserHandler(IUserRepository repository, IFileHelper fileHelper)
    {
        _userRepository= repository;
        _fileHelper= fileHelper;
    }
    public async Task<UserModel> Handle(AddAdminUserCommand request, CancellationToken cancellationToken)
    {
        ValidatorBehavior<AddAdminUserCommand>.Validate(request);

        string? picture = null;
        if(request.Picture is not null)
        {
            var result = await _fileHelper.SaveImageToServerAsync(request.Picture, new[] { "admin", "pictures" });
            picture = result.Item1;
        }

        var user = new User()
        {
            UserName= request.UserName,
            Email= request.Email,
            FirstName= request.FirstName,
            LastName= request.LastName,
            PhoneNumber= request.PhoneNumber
        };
        if (picture is not null) user.Picture = picture;

        var admin = await _userRepository.AddAsync(user, request.Password ?? DefaultParams.defaultPwd, Roles.ADMIN);

        return admin;
    }
}
