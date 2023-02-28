using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using MediatR;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace CRM.Core.Business.UseCases.AddOtherUser
{
    public class AddOtherUserHandler : IRequestHandler<AddOtherUserCommand, UserModel>
    {
        private readonly IUserRepository _repo;
        private readonly IFileHelper _fileHelper;
        private readonly ISkillRepository _skillRepository;
        public AddOtherUserHandler(IUserRepository userRepository, IFileHelper fileHelper, ISkillRepository skillRepository)
        {
            _repo = userRepository;
            _fileHelper = fileHelper;
            _skillRepository = skillRepository;
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
            if (request.User.Password == null) request.User.Password = DefaultParams.defaultPwd;
            var currentUserRoles = await _repo.GetUserAndRole(request.CurrentUserName);
            if (currentUserRoles == null) throw new UnauthorizedAccessException();

            var isActive = await _repo.IsActivatedUserAsync(request.CurrentUserName);
            var roles = currentUserRoles.Item2;

            if (roles == null || !isActive) throw new UnauthorizedAccessException();
            List<string?> listRoles = roles.Select(v => v.Name).ToList();
            UserBodyAndRole cur = CheckValidity(request, roles, listRoles!);
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
            AddSkills(cur, user);
            if (picture is not null) user.Picture = picture;
            var userAndRole = await _repo.AddAsync(user, request.User.Password ?? DefaultParams.defaultPwd, request.User.Role);
            await AddSkillsToDb(user, userAndRole);
            return userAndRole;
        }

        /// <summary>
        /// Add skills to database and add them to user created
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userAndRole"></param>
        /// <returns></returns>
        private async Task AddSkillsToDb(User user, UserModel userAndRole)
        {
            var skills1 = user.Experiences;
            var skills2 = user.Studies;
            user.Experiences = null;
            user.Studies = null;
            if (skills2 is not null && skills2.Any())
            {
                foreach(var s in skills2)
                {
                    s.Student = user;
                }
                var studies = await _skillRepository.AddRangeAsync(skills2);
                userAndRole.Studies = studies.Select(s => new SkillModel(s.Name, s.Place, s.IsCurrent, s.StartDate, s.EndDate)).ToList();
            }
            if (skills1 is not null && skills1.Any())
            {
                foreach (var s in skills1)
                {
                    s.Expert = user;
                }
                var xp = await _skillRepository.AddRangeAsync(skills1);
                userAndRole.Experiences = xp.Select(s => new SkillModel(s.Name, s.Place, s.IsCurrent, s.StartDate, s.EndDate)).ToList();
            }
            user.Studies = skills2;
            user.Experiences = skills1;
        }

        /// <summary>
        /// Add skills to user
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="user"></param>
        private static void AddSkills(UserBodyAndRole cur, User user)
        {
            if (cur.Studies != null && cur.Studies.Count > 0)
            {
                user.Studies = cur.Studies.ToList().Select(s => new Skill()
                {
                    Name = s.Name,
                    Place = s.Place,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsCurrent = s.IsCurrent
                }).ToList();
            }
            if (cur.Experiences != null && cur.Experiences.Count > 0)
            {
                user.Experiences = cur.Experiences.ToList().Select(s => new Skill()
                {
                    Name = s.Name,
                    Place = s.Place,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsCurrent = s.IsCurrent
                }).ToList();
            }
        }

        /// <summary>
        /// Check if request data are valid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="roles"></param>
        /// <param name="listRoles"></param>
        /// <returns>
        /// Current user
        /// </returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="BaseException"></exception>
        private static UserBodyAndRole CheckValidity(AddOtherUserCommand request, List<Role> roles, List<string> listRoles)
        {
            if (
                            (request.User.Role == Roles.ADMIN) ||
                            (listRoles.Contains(Roles.CLIENT)) ||
                            request.User.Role == Roles.SUPERVISOR && listRoles.Contains(Roles.SUPERVISOR)
                            ) throw new UnauthorizedAccessException();
            ValidatorBehavior<UserBodyAndRole>.Validate(request.User);

            var cur = request.User;
            var errors = new Dictionary<string, List<string>>();

            if (cur.Studies is not null && cur.Studies.Any()) CheckIfSkillsIsValid(cur.Studies, "Studies", errors);
            if (cur.Experiences is not null && cur.Experiences.Any()) CheckIfSkillsIsValid(cur.Experiences, "Experiences", errors);
            if (errors.Any()) throw new BaseException(errors);

            var isClient = roles.Find(r => r.Name == Roles.CLIENT) != null;
            var isSupervisor = roles.Find(r => r.Name == Roles.SUPERVISOR) != null;
            var isCCL = roles.Find(r => r.Name == Roles.CCL) != null;
            if (isClient) throw new UnauthorizedAccessException();
            if (isSupervisor && (request.User.Role == Roles.ADMIN || request.User.Role == Roles.SUPERVISOR)) throw new UnauthorizedAccessException();
            if (isCCL && (request.User.Role == Roles.ADMIN || request.User.Role == Roles.SUPERVISOR || request.User.Role == Roles.CCL)) throw new UnauthorizedAccessException();
            return cur;
        }

        /// <summary>
        /// Check if skills are valid
        /// </summary>
        /// <param name="skillModels"></param>
        /// <param name="fieldName"></param>
        /// <param name="errors"></param>
        private static void CheckIfSkillsIsValid(ICollection<SkillModel> skillModels, string fieldName, Dictionary<string, List<string>> errors)
        {
            var errorMsg = new List<string>
            {
                "start date".ToGreaterThanMsg("end Date")
            };
            foreach (var (skillModel, index) in skillModels.WithIndex())
            {
                skillModel.IsCurrent = skillModel.EndDate is null;
                if (skillModel.EndDate is null)
                {
                    continue;
                }
                if (skillModel.StartDate > skillModel.EndDate)
                {
                    errors.Add($"{fieldName}[{index}]", errorMsg);
                }
            }
        }
    }
}
