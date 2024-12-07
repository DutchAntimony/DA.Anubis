using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.Anubis.Domain.Contract.Enumerations;
using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.PersistenceLibrary.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class LidConfiguration : IEntityTypeConfiguration<Lid>
{
    public void Configure(EntityTypeBuilder<Lid> builder)
    {
        builder.ToTable("Leden").HasKey(lid => lid.Id);

        builder.Property(lid => lid.Id).ValueGeneratedNever().HasColumnOrder(0);

        builder.Property(lid => lid.Lidnummer).HasColumnOrder(1);

        builder.OwnsOne(lid => lid.Personalia, innerBuilder =>
        {
            innerBuilder.Property(personalia => personalia.Voorletters)
                .HasColumnName("Voorletters")
                .HasColumnOrder(2)
                .HasMaxLength(15);
            
            innerBuilder.Ignore(personalia => personalia.Tussenvoegsel);
            innerBuilder.Property<string?>("_tussenvoegsel")
                .HasColumnName("Tussenvoegsel")
                .HasColumnOrder(3)
                .HasMaxLength(15);

            innerBuilder.Property(personalia => personalia.Achternaam)
                .HasColumnName("Achternaam")
                .HasColumnOrder(4)
                .HasMaxLength(127);
            
            innerBuilder.Property(personalia => personalia.Geslacht)
                .HasColumnName("Geslacht")
                .HasColumnOrder(5)
                .HasConversion<int>();
            
            innerBuilder.Property(personalia => personalia.Geboortedatum)
                .HasColumnName("Geboortedatum")
                .HasColumnOrder(6)
                .HasColumnType("date");
        });
        
        builder.Ignore(lid => lid.AdresId);
        builder.Property<AdresId?>("_adresId")
            .HasColumnOrder(7)
            .HasColumnName("AdresId");
        
        builder.Ignore(lid => lid.BetaalmethodeId);
        builder.Property<BetaalmethodeId?>("_betaalmethodeId")
            .HasColumnOrder(8)
            .HasColumnName("BetaalmethodeId");

        builder.Ignore(lid => lid.Uitschrijfreden);
        builder.Property<Uitschrijfreden?>("_uitschrijfreden")
            .HasConversion<int>()
            .HasColumnOrder(9)
            .HasColumnName("Uitschrijfreden");
        
        //todo: add email, telefoon en notities.

        builder.Ignore(lid => lid.Mutaties);
        builder.HasMany<LidMutatie>("_mutaties")
            .WithOne()
            .HasForeignKey(mutatie => mutatie.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ConfigureDeletionInfo();
        
        builder
            .HasIndex("_adresId")
            .HasDatabaseName("IX_Lid_AdresId");
        
        builder
            .HasIndex("_betaalmethodeId")
            .HasDatabaseName("IX_Lid_BetaalmethodeId");
    }
}