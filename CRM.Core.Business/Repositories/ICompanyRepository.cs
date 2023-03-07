using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface ICompanyRepository
{
    Task<Company> AddOneAsync(Company company);
    Task DeleteManyAsync(ICollection<Company> companies);
    Task<ICollection<Company>> GetAllAsync();
    Task<ICollection<Company>> GetAllAsync(string userName);
    Task<ICollection<Company>> GetManyAsync(ICollection<Guid> ids);
    Task<ICollection<Company>> GetManyAsync(ICollection<Guid> ids, string userName);
    Task<Company?> GetOneAsync(Guid id);
    Task<Company?> GetOneAsync(Guid id, string userName);
    Task MarkAsDeletedAsync(ICollection<Company> companies);
    Task<Company> UpdateAsync(Company company);
    Task<ICollection<Company>> UpdateManyAsync(ICollection<Company> companies);
}
