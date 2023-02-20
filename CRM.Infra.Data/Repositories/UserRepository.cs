﻿using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CRM.Infra.Data.Repositories;

public class UserRepository: IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IFileHelper _fileHelper;
    public UserRepository(UserManager<User> userManager, RoleManager<Role> roleManager, IFileHelper fileHelper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _fileHelper = fileHelper;
    }

    /// <summary>
    /// Add new user and his role in the database
    /// </summary>
    /// <param name="user"></param>
    /// <param name="pwd"></param>
    /// <param name="role"></param>
    /// <returns>User and his roles</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when role in not a valid role
    /// </exception>

    public async Task<UserModel> AddAsync(User user, string pwd, string role)
    {
        if (
            role != Roles.CCL &&
            role != Roles.ADMIN &&
            role != Roles.SUPERVISOR &&
            role != Roles.CLIENT
            )
            throw new UnauthorizedAccessException();
        var u = await CreateUser(user, pwd);
        var addRoleResult = await AddRole(u, role);

        return addRoleResult;
    }

    /// <summary>
    /// Create user in database with his password
    /// </summary>
    /// <param name="user"></param>
    /// <param name="pwd"></param>
    /// <returns>User created in database</returns>
    /// <exception cref="BaseException">
    /// Throwm if the user could not be added because of invalid or existing information
    /// </exception>
    private async Task<User> CreateUser(User user, string pwd)
    {
        var studies = user.Studies;
        var xp = user.Experiences;
        user.Experiences = null;
        user.Studies = null;
        var identityResult = await _userManager.CreateAsync(user, pwd);
        if (!identityResult.Succeeded)
        {
            if (user.Picture is not null) _fileHelper.DeleteImageToServer(user.Picture);
            CheckIdentityResultAndThrowException(identityResult);
        }
        var u = await _userManager.FindByNameAsync(user.UserName!);
        user.Studies = studies;
        user.Experiences = xp;
        return u!;
    }

    private static void CheckIdentityResultAndThrowException(IdentityResult identityResult)
    {
        var errors = new Dictionary<string, List<string>>();
        foreach (var error in identityResult.Errors)
        {
            switch (error.Code)
            {
                case "DuplicateEmail":
                    errors
                        .AddOrCreate(new KeyValuePair<string, string>("Email", "email".ToAlReadyExistMsg()));
                    break;
                case "InvalidEmail":
                    errors
                        .AddOrCreate(new KeyValuePair<string, string>("Email", "email".ToInvalidMsg()));
                    break;
                case "DuplicateUserName":
                    errors
                        .AddOrCreate(new KeyValuePair<string, string>("UserName", "username".ToAlReadyExistMsg()));
                    break;
                case "InvalidUserName":
                    errors
                        .AddOrCreate(new KeyValuePair<string, string>("UserName", "username".ToInvalidMsg()));
                    break;
                case "UserAlreadyInRole":
                    errors
                        .AddOrCreate(new KeyValuePair<string, string>("Role", "This user already in role !"));
                    break;
                case "DefaultError":
                    errors
                        .AddOrCreate(new KeyValuePair<string, string>("Default", "Something went wrong !"));
                    break;
                default:
                    break;

            }
        }
        throw new BaseException(errors);
    }

    /// <summary>
    /// Add role to user in database
    /// </summary>
    /// <param name="u"></param>
    /// <param name="role"></param>
    /// <returns>User and his roles</returns>
    /// <exception cref="BaseException"></exception>
    private async Task<UserModel> AddRole(User u, string role)
    {
        var studies = u.Studies;
        var xp = u.Experiences;
        u.Experiences = null;
        u.Studies = null;
        var roleExist = await _roleManager.RoleExistsAsync(role);
        if (!roleExist)
        {
            await _roleManager.CreateAsync(new Role() { Name= role});
        }
        var roleResult = await _userManager.AddToRoleAsync(u, role);
        var errors = new Dictionary<string, List<string>>();
        if (!roleResult.Succeeded)
        {
            throw new BaseException(errors);
        }

        u.Studies = studies;
        u.Experiences = xp;
        return new UserModel
        {
            Id = u.Id,
            Email = u.Email ?? "",
            FirstName = u.FirstName,
            UserName = u.UserName!,
            LastName = u.LastName,
            Picture = u.Picture,
            PhoneNumber = u.PhoneNumber,
            CreatedAt = u.CreatedAt,
            UpdateAt = u.UpdateAt,
            DeletedAt = u.DeletedAt,
            Roles = new List<string> { role },
        };
    }

    /// <summary>
    /// Get user on database by his credentials (Username and passwor)
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>A tuple of user and his roles</returns>
    public async Task<Tuple<User, List<Role>>?> GetByUserAndRoleAsync(string username, string password)
    {
        var user = await FindByNameAsync(username);
        if (user is null) return null;
        var isCorrectPwd = await _userManager.CheckPasswordAsync(user, password);
        if (!isCorrectPwd) return null;
        var roles = await _userManager.GetRolesAsync(user);
        if(roles is null) return null;
        var roleEntities = new List<Role>();
        foreach (var roleEntity in roles)
        {
            var r = await _roleManager.FindByNameAsync(roleEntity);
            if(r is not null) roleEntities.Add(r);
        }
        return new Tuple<User, List<Role>>(user, roleEntities);
    }

    /// <summary>
    /// Get user and his role by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns>A tuple of user and his roles</returns>
    public async Task<Tuple<User, List<Role>>?> GetUserAndRole(string username)
    {
        var user = await FindByNameAsync(username);
        if(user is null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        if (roles is null) return null;
        var roleEntities = new List<Role>();
        foreach (var roleEntity in roles)
        {
            var r = await _roleManager.FindByNameAsync(roleEntity);
            if (r is not null) roleEntities.Add(r);
        }

        return new Tuple<User, List<Role>>(user, roleEntities);
    }

    /// <summary>
    /// Convert user to user model
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roles"></param>
    /// <returns>User model instance</returns>
    public UserModel UserToUserModel(User user, List<Role> roles)
    {
        return new UserModel(
            user.Id,
            user.UserName ?? "",
            user.Email ?? "",
            user.FirstName,
            user.LastName,
            roles.Select(r => r.Name).ToList(),
            user.Picture,
            user.PhoneNumber,
            user.CreatedAt,
            user.UpdateAt,
            user.DeletedAt);
    }

    /// <summary>
    /// Add users to database using list of user csv model and his roles
    /// </summary>
    /// <param name="users"></param>
    /// <param name="role"></param>
    /// <param name="creatorUserName"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException">
    /// When current user not exist or role is not a known role
    /// </exception>
    public async Task<List<UserCsvModel>> AddFromListAsync(List<UserCsvModel> users, string role, string creatorUserName)
    {
        if(role == Roles.ADMIN) throw new UnauthorizedAccessException();
        if(role != Roles.CCL && role != Roles.SUPERVISOR && role != Roles.CLIENT) throw new UnauthorizedAccessException();
        var currentUserRoles = await GetUserAndRole(creatorUserName);
        if(currentUserRoles == null) throw new UnauthorizedAccessException();
        var roles = currentUserRoles.Item2;
        var isClient = roles.Find(r => r.Name == Roles.CLIENT) != null;
        var isSupervisor = roles.Find(r => r.Name == Roles.SUPERVISOR) != null;
        var isCCL = roles.Find(r => r.Name == Roles.CCL) != null;
        if (isClient) throw new UnauthorizedAccessException();
        if(isSupervisor && (role == Roles.ADMIN || role == Roles.SUPERVISOR)) throw new UnauthorizedAccessException();
        if(isCCL && (role == Roles.ADMIN || role == Roles.SUPERVISOR || role == Roles.CCL)) throw new UnauthorizedAccessException();


        foreach (var user in users)
        {
            if(user.Status == FIleReadStatus.Valid)
            {
                var u = new User()
                {
                    UserName= user.UserName ?? "",
                    Email = user.Email ?? "",
                    FirstName= user.FirstName ?? "",
                    LastName= user.LastName ?? "",
                    PhoneNumber= user.PhoneNumber ?? "",
                    Creator = currentUserRoles.Item1
                };
                try
                {
                    var nU = await CreateUser(u, DefaultParams.defaultPwd);
                    var addRoleResult = await AddRole(nU, role);
                    user.CreatedAt = nU.CreatedAt;
                }
                catch(BaseException ex)
                {
                    user.Errors = ex.Errors;
                    user.Status = FIleReadStatus.Exist;
                }
            }

        }
        return users;
    }

    /// <summary>
    /// Get users by the username of their creator
    /// </summary>
    /// <param name="creatorUserName">User name of creator</param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException">When creator not exist.</exception>
    public async Task<ICollection<UserAndCreatorModel>> GetUsersByCreatorUserNameAsync(string creatorUserName)
    {
        var creator = await FindByNameAsync(creatorUserName);
        if (creator == null) throw new UnauthorizedAccessException();
        var userRoles = await _userManager.GetRolesAsync(creator);
        if(userRoles.Any(ur => ur == Roles.ADMIN))
        {
            return _userManager
                .Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => !u.UserRoles.Any(ur => ur.Role.Name == Roles.ADMIN) && u.DeletedAt == null)
                .Select(u => ConvertUserAndCreator(u, creator)).ToList(); ;
        }
        return _userManager
                .Users
                .Include(u => u.Creator)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.Creator != null && u.Creator.UserName == creator.UserName && u.DeletedAt == null)
                .Select(u => ConvertUserAndCreator(u, creator)).ToList();
    }

    private static UserAndCreatorModel ConvertUserAndCreator(User u, User creator)
    {
        List<string>? roles = GetRolesInUserRoleList(u);
        UserAndCreatorModel uandc = Conversion(u, creator);
        if (roles is not null && roles.Count > 0) uandc.Roles = roles;
        return uandc;
    }

    /// <summary>
    /// Convert user to UserAndCreatorModel
    /// </summary>
    /// <param name="u"></param>
    /// <param name="creator"></param>
    /// <returns>Convertion version of user to UserAndCreatorModel</returns>
    private static UserAndCreatorModel Conversion(User u, User creator)
    {
        return new UserAndCreatorModel(
            u.Id,
            u.UserName!,
            u.Email!,
            u.FirstName,
            u.LastName,
            new List<string>(),
            u.Picture,
            u.PhoneNumber,
            u.CreatedAt,
            u.UpdateAt,
            u.DeletedAt,
            new BaseUserModel(
                creator.Id,
                creator.UserName!,
                creator.Email!,
                creator.FirstName,
                creator.LastName,
                new List<string>(),
                creator.Picture,
                creator.PhoneNumber,
                creator.CreatedAt,
                creator.UpdateAt,
                creator.DeletedAt
                )
            );
    }
    /// <summary>
    /// Extract roles of user in his userRole list.
    /// </summary>
    /// <param name="u"></param>
    /// <returns>List of user role to string</returns>
    private static List<string> GetRolesInUserRoleList(User u)
    {
        return u.UserRoles.Aggregate(new List<string>(), (acc, x) =>
        {
            if (x.Role.Name is not null) acc.Add(x.Role.Name);
            return acc;
        });
    }

    /// <summary>
    /// Mark users as delted on the db
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task MarkAsDeletedRangeAsync(List<Guid> ids, string username)
    {
        var users = new List<User>();
        await GetUserToDelete(ids, username, users);
        await MarkUseAsDeleted(users);
    }

    /// <summary>
    /// Mark user as deleted
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    private async Task MarkUseAsDeleted(List<User> users)
    {
        foreach (var user in users)
        {
            user.DeletedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }
    }

    /// <summary>
    /// Finds all user to delete in data base en throw exception when 
    /// One user not exist or creator not have the same username a like params username
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="username"></param>
    /// <param name="users"></param>
    /// <returns></returns>
    /// <exception cref="BaseException"></exception>
    private async Task GetUserToDelete(List<Guid> ids, string username, List<User> users)
    {
        var creator = await GetUserAndRole(username);
        if (creator == null) throw new UnauthorizedAccessException();
        foreach (Guid id in ids)
        {
            var user =
                await _userManager
                .Users
                .Include(u => u.Creator)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (
                user == null || 
                (user.Creator?.UserName != username && !creator.Item2.Any(r => r.Name == Roles.ADMIN))) throw new BaseException(new Dictionary<string, List<string>>
            {
                { id.ToString(), new List<string>{ "User is not defined !" } }
            });
            users.Add(user);
        }
    }

    /// <summary>
    /// Find user not deleted
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private async Task<User?> FindByNameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null || user.DeletedAt != null) return null;

        return user;
    }

    public Task<User?> GetUserAndRolesAsync(Guid id)
    {
        return _userManager
            .Users
            .Include(u => u.Creator)
            .Include(u => u.Experiences)
            .Include(u => u.Studies)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.DeletedAt == null && u.Id == id)
            .FirstOrDefaultAsync();
    }

    public Task<User?> GetUserAndRolesAsync(string username)
    {
        return _userManager
            .Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.DeletedAt == null && u.UserName == username)
            .FirstOrDefaultAsync();
    }

    public Task<User?> GetUserAndRolesAsync(Guid id, string creatorUserName)
    {
        return _userManager
            .Users
            .Include(u => u.Creator)
            .Include(u => u.Experiences)
            .Include(u => u.Studies)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.DeletedAt == null && u.Id == id && u.Creator != null && u.Creator.UserName == creatorUserName)
            .FirstOrDefaultAsync();
    }

    public async Task SetUserPasswordAsync(User user, string newPassword, string? oldPassword)
    {
        var isPwdValid = await _userManager.CheckPasswordAsync(user, oldPassword);
        if(isPwdValid)
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);
            return;
        }
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>
        {
            {"Password", new List<string>{"Invalid password"} }
        };
        throw new BaseException(errors);
    }

    public async Task SetUserRolesAsync(User user, List<string> roles)
    {
        foreach (var role in roles)
        {
            var isExistRole = await _roleManager.RoleExistsAsync(role);
            if (!isExistRole)
            {

                Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>
                {
                    {"Role", new List<string>{$"Role {role} not exist !"} }
                };
                throw new BaseException(errors);
            }
        }
        await _userManager.RemoveFromRolesAsync(user, user.UserRoles.Select(r => r.Role.Name)!);
        await _userManager.AddToRolesAsync(user, roles);
    }

    public async Task SetUserSkillsAsync(User userToUpdate, ICollection<SkillModel> collection, bool isStudies)
    {
        if (isStudies)
        {
            userToUpdate.Studies?.Clear();
            userToUpdate.Studies = collection.Select(u => new Skill
            {
                StartDate = u.StartDate,
                EndDate = u.EndDate,
                Name = u.Name,
                Place = u.Place,
                IsCurrent = u.IsCurrent,
            }).ToList();
        }
        else
        {
            userToUpdate.Experiences?.Clear();
            userToUpdate.Experiences = collection.Select(u => new Skill
            {
                StartDate = u.StartDate,
                EndDate = u.EndDate,
                Name = u.Name,
                Place = u.Place,
                IsCurrent = u.IsCurrent,
            }).ToList();
        }
        await _userManager.UpdateAsync(userToUpdate);
    }

    public async Task<User> SetUserSimpleData(User userToUpdate)
    {
        var result = await _userManager.UpdateAsync(userToUpdate);
        if (!result.Succeeded)
            CheckIdentityResultAndThrowException(result);
        return (await GetUserAndRolesAsync(userToUpdate.Id))!;
    }
}
