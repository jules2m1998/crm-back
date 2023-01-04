using CRM.api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Data
{
    public class ApplicationDbContext: IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions dbContextOption) : base(dbContextOption) { }
    }
}
