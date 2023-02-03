using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRM.Infra.Data.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserRepository(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserModel> AddAsync(User user, string pwd, string role)
        {
            var u = await CreateUser(user, pwd);
            var addRoleResult = await AddRole(u, role);

            return addRoleResult;
        }

        private async Task<User> CreateUser(User user, string pwd)
        {
            var identityResult = await _userManager.CreateAsync(user, pwd);
            var errors = new Dictionary<string, List<string>>();
            if (!identityResult.Succeeded)
            {
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
            return await _userManager.FindByNameAsync(user.UserName!) ?? new User();
        }

        private async Task<UserModel> AddRole(User u, string role)
        {
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

        public async Task<Tuple<User, List<Role>>?> GetByUserAndRoleAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
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

        public async Task<Tuple<User, List<Role>>?> GetUserAndRole(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return null;
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

        public UserModel UserToUserModel(User user, List<Role> roles)
        {
            return new UserModel(
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                roles.Select(r => r.Name).ToList(),
                user.Picture,
                user.PhoneNumber,
                user.CreatedAt,
                user.UpdateAt,
                user.DeletedAt);
        }

        public async Task<List<UserCsvModel>> AddFromListAsync(List<UserCsvModel> users, string role)
        {
            if(role == Roles.ADMIN) throw new UnauthorizedAccessException();
            if(role != Roles.CCN) throw new UnauthorizedAccessException();
            foreach(var user in users)
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
    }
}
