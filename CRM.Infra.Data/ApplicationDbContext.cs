using CRM.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infra.Data
{
    public class ApplicationDbContext: IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions): base(dbContextOptions) { }
    }
}
