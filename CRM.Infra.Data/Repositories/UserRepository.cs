using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace CRM.Infra.Data.Repositories
{
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
            var identityResult = await _userManager.CreateAsync(user, pwd);
            var errors = new Dictionary<string, List<string>>();
            if (!identityResult.Succeeded)
            {
                if(user.Picture is not null) _fileHelper.DeleteImageToServer(user.Picture);
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

        /// <summary>
        /// Add role to user in database
        /// </summary>
        /// <param name="u"></param>
        /// <param name="role"></param>
        /// <returns>User and his roles</returns>
        /// <exception cref="BaseException"></exception>
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

        /// <summary>
        /// Get user on database by his credentials (Username and passwor)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>A tuple of user and his roles</returns>
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

        /// <summary>
        /// Get user and his role by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A tuple of user and his roles</returns>
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
    }
}
