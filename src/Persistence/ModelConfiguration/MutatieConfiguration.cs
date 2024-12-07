using DA.Anubis.Domain;
using DA.Anubis.Domain.AdresAggregate;
using DA.Anubis.Domain.BetaalmethodeAggregate;
using DA.Anubis.Domain.LedenAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class MutatieConfiguration : IEntityTypeConfiguration<Mutatie>
{
    public void Configure(EntityTypeBuilder<Mutatie> builder)
    {
        builder.ToTable("Mutaties").HasKey(mutatie => mutatie.Id);

        builder.Property(mutatie => mutatie.Id).ValueGeneratedNever().HasColumnOrder(0);
        
        builder.HasDiscriminator<string>("MutatieType")
            .HasValue<AdresMutatie>(nameof(Adres))
            .HasValue<BetaalmethodeMutatie>(nameof(Betaalmethode))
            .HasValue<LidMutatie>(nameof(Lid));

        builder.Property<string?>("_oldValue").HasColumnName("Old value").HasColumnOrder(4);
        builder.Property<string?>("_newValue").HasColumnName("New value").HasColumnOrder(5);
    }
}