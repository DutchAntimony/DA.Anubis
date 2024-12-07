using DA.Anubis.Domain.AdresAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

internal sealed class AdresMutatieConfiguration : IEntityTypeConfiguration<AdresMutatie>
{
    public void Configure(EntityTypeBuilder<AdresMutatie> builder)
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