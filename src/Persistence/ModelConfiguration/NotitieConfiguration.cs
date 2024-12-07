using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.PersistenceLibrary.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class NotitieConfiguration : IEntityTypeConfiguration<Notitie>
{
    public void Configure(EntityTypeBuilder<Notitie> builder)
    {
        builder.ToTable("Notities").HasKey(notitie => notitie.Id);
        
        builder.Property(notitie => notitie.Id)
            .HasConversion<ValueConverters.EntityKeyConverter<NotitieId>>();
    }
}