using DA.Anubis.Domain.LedenAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class LidMutatieConfiguration : IEntityTypeConfiguration<LidMutatie>
{
    public void Configure(EntityTypeBuilder<LidMutatie> builder)
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