using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> AddRangeAsync(IEnumerable<Skill> tasks);
    }
}
