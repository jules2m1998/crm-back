using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Configurations
{
    public class CommitConfigruration : IEntityTypeConfiguration<Commit>
    {
        public void Configure(EntityTypeBuilder<Commit> builder)
        {
            builder.HasOne(x => x.Response).WithMany().OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(x => x.Parent).WithMany().OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
