using DA.Anubis.Domain.BetaalmethodeAggregate;
using DA.Anubis.Domain.Contract.AggregateKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class BetaalmethodeConfiguration : IEntityTypeConfiguration<Betaalmethode>
{
    public void Configure(EntityTypeBuilder<Betaalmethode> builder)
    {
        builder.ToTable("Betaalmethodes").HasKey(betaalmethode => betaalmethode.Id);
        
        builder.Property(betaalmethode => betaalmethode.Id).ValueGeneratedNever();
        
        builder.HasDiscriminator<string>("Methode")
            .HasValue<NotaBetaalmethode>("Nota")
            .HasValue<IncassoBetaalmethode>("Incasso");
        
        builder.Ignore(betaalmethode => betaalmethode.VerantwoordelijkLidId);
        builder.Property<LidId?>("_verantwoordelijkLidId").HasColumnName("VerantwoordelijkLidId");

        builder.Ignore(betaalmethode => betaalmethode.Mutaties);
        builder.HasMany<BetaalmethodeMutatie>("MutatieCollectie")
            .WithOne()
            .HasForeignKey(mutatie => mutatie.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex("_verantwoordelijkLidId")
            .HasDatabaseName("IX_Adres_VerantwoordelijkLidId");
    }
}