using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infra.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyContact> CompanyContacts { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Prospect> Prospects { get; set; }
        public DbSet<ProspectionHistory> ProspectionHistories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}