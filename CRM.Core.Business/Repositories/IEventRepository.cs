using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IEventRepository
{
    Task AddAsync(Event e);
    Task<Event?> GetAsync(Guid id, string userName);
    Task<Event?> GetAsync(Guid id);
    Task<ICollection<Event>> GetByUserAsync(string userName);
}
