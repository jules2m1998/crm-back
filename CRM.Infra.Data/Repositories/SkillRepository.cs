using CRM.Core.Business;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using CRM.Core.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infra.Data.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly DbSet<Skill> _skillSet;

        public SkillRepository(IApplicationDbContext context)
        {
            _context = context;
            _skillSet = _context.Skills;
        }

        /// <summary>
        /// Add skills
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Skill>> AddRangeAsync(IEnumerable<Skill> tasks)
        {
            foreach (var (task, index) in tasks.WithIndex())
            {
                ValidatorBehavior<Skill>.Validate(task);
                if (task.Expert is null && task.Student is null)
                {
                    var errors = new Dictionary<string, List<string>>
                    {
                        {
                            $"Expert[{index}]",
                            new List<string>
                            {
                                "Expert".ToTwoNotEmptyMsg("Student")
                            }
                            },
                        {
                            $"Student[{index}]",
                            new List<string>
                            {
                                "Student".ToTwoNotEmptyMsg("Expert")
                            }
                        }
                    };
                    throw new BaseException(errors);
                }
            }
            await _skillSet.AddRangeAsync(tasks);
            var r = await _context.SaveChangesAsync();
            return tasks.ToList();
        }
    }
}
