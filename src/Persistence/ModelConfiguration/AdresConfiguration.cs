using DA.Anubis.Domain.AdresAggregate;
using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.PersistenceLibrary.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

internal sealed class AdresConfiguration : IEntityTypeConfiguration<Adres>
{
    public void Configure(EntityTypeBuilder<Adres> builder)
    {
        builder.ToTable("Adressen").HasKey(adres => adres.Id);
        
        builder.Property(adres => adres.Id).HasColumnOrder(0).ValueGeneratedNever();
        
        builder.Ignore(adres => adres.HoofdbewonerId);
        builder.Property<LidId?>("_hoofdbewonerId")
            .HasColumnName("HoofdbewonerId")
            .HasColumnOrder(6);
        
        builder.Property(adres => adres.Straatnaam).HasColumnOrder(1).HasMaxLength(100);
        builder.Property(adres => adres.Huisnummer).HasColumnOrder(2).HasMaxLength(10);
        builder.Property(adres => adres.Postcode).HasColumnOrder(3).HasMaxLength(10);
        builder.Property(adres => adres.Woonplaats).HasColumnOrder(4).HasMaxLength(100);

        builder.Ignore(adres => adres.Land);
        builder.Property<string?>("_land")
            .HasColumnName("Land")
            .HasMaxLength(5)
            .HasColumnOrder(5);

        builder.HasMany<Lid>()
            .WithOne()
            .HasForeignKey("_adresId")
            .OnDelete(DeleteBehavior.SetNull);

        builder.Ignore(adres => adres.Mutaties);
        builder.HasMany<AdresMutatie>("_mutaties")
            .WithOne()
            .HasForeignKey(mutatie => mutatie.EntityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureDeletionInfo();
        
        builder.HasIndex("_hoofdbewonerId")
            .HasDatabaseName("IX_Adres_HoofdbewonerId");
    }
}