using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infra.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Skill> Skills { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}