using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.PersistenceLibrary.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class TelefoonConfiguration : IEntityTypeConfiguration<Telefoonnummer>
{
    public void Configure(EntityTypeBuilder<Telefoonnummer> builder)
    {
        builder.ToTable("Telefoonnummers").HasKey(telefoonnummer => telefoonnummer.Id);
        
        builder.Property(telefoonnummer => telefoonnummer.Id)
            .HasConversion<ValueConverters.EntityKeyConverter<TelefoonnummerId>>();
    }
}