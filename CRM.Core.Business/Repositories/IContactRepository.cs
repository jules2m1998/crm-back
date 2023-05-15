using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IContactRepository
{
    Task AddAsync(Contact contact, CancellationToken cancellationToken);
    Task<ICollection<Contact>> GetAsync();
    Task<Contact?> GetAsync(string name, Guid companyId, CancellationToken cancellationToken);
    Task<ICollection<Contact>> GetMineAsync(Guid userId);
    Task<Contact?> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<Contact?> GetOneAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task RemoveAsync(Contact contact);
    Task UpdateAsync(Contact contact);
}
