using CRM.Core.Business.Delegates;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserModel?>
{
    private readonly IUserRepository _repo;
    private readonly IFileHelper _fileManager;

    public UpdateUserHandler(IUserRepository repo, IFileHelper fileManager)
    {
        _repo = repo;
        _fileManager = fileManager;
    }

    public async Task<UserModel?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, List<string>>();
        var user = request.User;
        var userName = request.UserName;
        if (string.IsNullOrEmpty(userName) || user is null)
        {
            if (user is null) errors.Add("User", new List<string> { "This field is required !" });
            if (string.IsNullOrEmpty(userName)) errors.Add("UserName", new List<string> { "This field is required !" });
            throw new BaseException(errors);
        }
        if (user.NewPassword is not null && user.OldPassword is null)
            errors.Add("Password", new List<string> { "Password not confirmed !" });
        ValidatorBehavior<UserUpdateModel>.Validate(user);
        if (errors.Any()) throw new BaseException(errors);

        // Get user with their skills, roles, and creator 
        var userToUpdate = await _repo.GetUserAndRolesAsync(user.Id, userName);

        // Update picture
        if (userToUpdate is null) return null;
        userToUpdate.UpdateAt = DateTime.UtcNow;
        if (user.Picture is not null)
        {
            if (userToUpdate.Picture != DefaultParams.defaultUserPicture)
                _fileManager.DeleteImageToServer(userToUpdate.Picture);
            var role = string.Join("_", user.Roles);
            var pic = await _fileManager.SaveImageToServerAsync(user.Picture, new[] { role, "pictures" });
            userToUpdate.Picture = pic.Item1 ?? DefaultParams.defaultUserPicture;
        }

        // Update password
        if (user.NewPassword is not null)
            await _repo.SetUserPasswordAsync(user: userToUpdate, user.NewPassword, user.OldPassword);

        // Update roles
        var userRoleString = userToUpdate.UserRoles.Select(x => x.Role.Name).ToList();
        if (!IsArrayAreSame(user.Roles, userRoleString!, (r1, r2) => r1 == r2))
        {
            await _repo.SetUserRolesAsync(user: userToUpdate, user.Roles);
        }

        // Set skills
        // - Studies
        var studiesAsSkill = userToUpdate.Studies!.Select(st => SkillToSkillModel(st)).ToList();
        EqualityDelegate<SkillModel> skillsEquals = (s1, s2) => s1.Equals(s2);
        if (!IsArrayAreSame(user.Studies!.ToList(), studiesAsSkill, skillsEquals))
        {
            await _repo.SetUserSkillsAsync(userToUpdate, collection: user.Studies!, isStudies: true);
        }
        // - Works
        var xpAsSkill = userToUpdate.Experiences!.Select(st => SkillToSkillModel(st)).ToList();
        if (!IsArrayAreSame(user.Experiences!.ToList(), xpAsSkill, skillsEquals))
        {
            await _repo.SetUserSkillsAsync(userToUpdate, collection: user.Experiences!, isStudies: false);
        }

        // Update other informations
        user.CopyToUser(userToUpdate);
        var nUser = await _repo.SetUserSimpleData(userToUpdate);

        return new UserModel
        {
            Id = nUser.Id,
            FirstName = nUser.FirstName,
            UserName = nUser.UserName!,
            LastName = nUser.LastName,
            Email = nUser.Email!,
            Studies = nUser.Studies!.Select(st => SkillToSkillModel(st)).ToList(),
            Experiences = nUser.Experiences!.Select(st => SkillToSkillModel(st)).ToList(),
            Roles = user.Roles,
            Picture = nUser.Picture,
            PhoneNumber = nUser.PhoneNumber,
            CreatedAt = nUser.CreatedAt,
            UpdateAt = nUser.UpdateAt,
            DeletedAt = nUser.DeletedAt
        };

    }

    private static SkillModel SkillToSkillModel(Skill st)
    {
        return new SkillModel { EndDate = st.EndDate, StartDate = st.StartDate, IsCurrent = st.IsCurrent, Name = st.Name, Place = st.Place };
    }

    private static bool IsArrayAreSame<T>(List<T> l1, List<T> l2, EqualityDelegate<T> equality)
    {
        var areEquals = false;
        if(l1.Count != l2.Count) return false;
        foreach (var role in l1)
        {
            areEquals = l2.Any(ul => equality(ul, role));
            if (!areEquals) return false;
        }
        return areEquals;
    }
}
