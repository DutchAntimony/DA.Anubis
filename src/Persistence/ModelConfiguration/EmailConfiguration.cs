using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.PersistenceLibrary.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class EmailConfiguration : IEntityTypeConfiguration<Emailadres>
{
    public void Configure(EntityTypeBuilder<Emailadres> builder)
    {
        builder.ToTable("Emailadressen").HasKey(emailadres => emailadres.Id);
        
        builder.Property(emailadres => emailadres.Id)
            .HasConversion<ValueConverters.EntityKeyConverter<EmailadresId>>();
    }
}