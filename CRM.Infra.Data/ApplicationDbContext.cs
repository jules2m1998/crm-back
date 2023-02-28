using CRM.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CRM.Infra.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IApplicationDbContext
    {
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyContact> CompanyContacts { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Prospect> Prospects { get; set; }
        public DbSet<ProspectionHistory> ProspectionHistories { get; set; }

        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User foreign keys management
            builder.Entity<CompanyContact>()
                .Property(p => p.Phones)
                .HasConversion(
                    from => string.Join(';', from),
                    to => string.IsNullOrEmpty(to) ? new List<string>() : to.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

            builder.Entity<User>()
                .HasMany(u => u.Prospects)
                .WithOne(p => p.Agent)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(u => u.ProspectionsHistories)
                .WithOne(p => p.User) 
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                .HasMany(u => u.Reattributions)
                .WithOne(r => r.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<User>()
                .HasMany(u => u.Studies)
                .WithOne(u => u.Student)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<User>()
                .HasMany(u => u.Experiences)
                .WithOne(u => u.Expert)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Role foreign keys management
            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // Prospect foreign keys management
            builder.Entity<Prospect>(prospect =>
            {
                prospect.HasKey(pr => new { pr.ProductId, pr.CompanyId });
                prospect.HasOne(p => p.Product)
                    .WithMany(ps => ps.Prospections)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                prospect.HasOne(p => p.Company)
                    .WithMany(p => p.Prospections)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
                prospect.HasOne(p => p.Creator)
                    .WithMany(u => u.ProspectionCreated)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<Company>()
                .HasOne(c => c.Creator)
                .WithMany(cr => cr.CreatedCompanies)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ProspectionHistory>(phistory =>
            {
                phistory.HasOne(ph => ph.Prospect)
                    .WithMany(p => p.History)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
