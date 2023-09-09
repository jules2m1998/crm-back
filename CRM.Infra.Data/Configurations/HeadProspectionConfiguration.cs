using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Configurations;

public class HeadProspectionConfiguration : IEntityTypeConfiguration<HeadProspection>
{
    public void Configure(EntityTypeBuilder<HeadProspection> builder)
    {
        builder.HasIndex(x => new { x.AgentId, x.ProductId, x.CompanyId }).IsUnique();
    }
}
