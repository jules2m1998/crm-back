using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities
{
    public class UserRole: IdentityUserRole<Guid>
    {
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
