using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.PersistenceLibrary.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class TelefoonnummerConfiguration : IEntityTypeConfiguration<Telefoonnummer>
{
    public void Configure(EntityTypeBuilder<Telefoonnummer> builder)
    {
        builder.ToTable("Telefoonnummers").HasKey(telefoonnummer => telefoonnummer.Id);
        
        builder.Property(telefoonnummer => telefoonnummer.Id)
            .HasConversion<ValueConverters.EntityKeyConverter<TelefoonnummerId>>()
            .ValueGeneratedNever()
            .HasColumnOrder(0);
        
        builder.Property(telefoonnummer => telefoonnummer.LidId).HasColumnOrder(1);

        builder.Property(telefoonnummer => telefoonnummer.Value).HasColumnOrder(2);
        
        builder.Property(telefoonnummer => telefoonnummer.Ordinal).HasColumnOrder(3);
    }
}