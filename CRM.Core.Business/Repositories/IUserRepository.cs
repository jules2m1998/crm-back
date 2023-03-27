using CRM.Core.Business.Models;
using CRM.Core.Domain.Entities;

namespace CRM.Core.Business.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> AddAsync(User user, string pwd, string role);
        Task<Tuple<User, List<Role>>?> GetByUserAndRoleAsync(string username, string password);
        Task<Tuple<User, List<Role>>?> GetUserAndRole(string username);
        UserModel UserToUserModel(User user, List<Role> roles);
        Task<List<UserCsvModel>> AddFromListAsync(List<UserCsvModel> users, string role, string creatorUserName);
        Task<ICollection<UserAndCreatorModel>> GetUsersByCreatorUserNameAsync(string creatorUserName);
        Task MarkAsDeletedRangeAsync(List<Guid> ids, string username);
        Task<User?> GetUserAndRolesAsync(string username);
        Task<User?> GetUserAndRolesAsync(Guid id, string creatorUserName);
        Task SetUserSkillsAsync(User userToUpdate, ICollection<SkillModel> collection, bool isStudies);
        Task<User> SetUserSimpleData(User userToUpdate);
        Task SetUserPasswordAsync(User user, string newPassword, string? oldPassword);
        Task SetUserRolesAsync(User user, List<string> roles);
        Task ResetUserPassword(User user);
        /// <summary>
        /// Get list of user created by creator username and have his id in ids list
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="creatorUserName"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <returns></returns>
        Task<ICollection<User>> GetUsersByCreatorUsernameAndIdsAsync(ICollection<Guid> ids, string creatorUserName);
        /// <summary>
        /// Toogle activated state of user
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        Task<ICollection<User>> ToogleUsersActivationStatus(ICollection<User> users);

        Task<bool> IsActivatedUserAsync(string userName);
        bool IsAdminUser(User user);
        Task<User?> GetUserAndSupervisedAsync(Guid id);
        Task<User?> GetUserAndSupervisedAsync(Guid id, string userName);
        Task<ICollection<User>> GetManyUserAndRoleAsync(ICollection<Guid> userIds);
        Task<ICollection<User>> GetManyUserAndRoleAsync(ICollection<Guid> userIds, string userName);
        Task<User> UpadeteUserAsync(User supervisor);
        Task<ICollection<User>> GetManyCCLUserToSupervisionAsync(ICollection<Guid> userIds, Guid supervisorId);
        Task<ICollection<User>> GetManyCCLUserToSupervisionAsync(ICollection<Guid> userIds, Guid supervisorId, string userName);
        Task<User?> GetUserByRoleAsync(Guid userId, string role);
        Task<ICollection<User>> GetUserByRoleAsync(string role);
        Task<ICollection<User>> GetUserByRoleAsync(string role, string userName);
    }
}
