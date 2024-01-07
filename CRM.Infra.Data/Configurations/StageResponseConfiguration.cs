using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infra.Data.Configurations;

public class StageResponseConfiguration : IEntityTypeConfiguration<StageResponse>
{
    public void Configure(EntityTypeBuilder<StageResponse> builder)
    {
        builder
            .HasOne(x => x.Stage)
            .WithMany(x => x.Responses)
            .HasForeignKey(x => x.StageId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder
            .HasOne(x => x.NextStage)
            .WithMany()
            .HasForeignKey(x => x.NextStageId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
