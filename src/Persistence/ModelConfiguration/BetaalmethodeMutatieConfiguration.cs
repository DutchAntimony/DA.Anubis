using DA.Anubis.Domain.BetaalmethodeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class BetaalmethodeMutatieConfiguration : IEntityTypeConfiguration<BetaalmethodeMutatie>
{
    public void Configure(EntityTypeBuilder<BetaalmethodeMutatie> builder)
    {
        builder.Property(mutatie => mutatie.EntityId)
            .HasColumnName("EntityId")
            .IsRequired()
            .HasColumnOrder(2);
        
        builder.Property(mutatie => mutatie.MutatieType)
            .HasConversion<string>()
            .HasColumnName("Type")
            .HasColumnOrder(3);
    }
}