using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.PersistenceLibrary.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class EmailadresConfiguration : IEntityTypeConfiguration<Emailadres>
{
    public void Configure(EntityTypeBuilder<Emailadres> builder)
    {
        builder.ToTable("Emailadressen").HasKey(emailadres => emailadres.Id);
        
        builder.Property(email => email.Id)
            .HasConversion<ValueConverters.EntityKeyConverter<EmailadresId>>()
            .ValueGeneratedNever()
            .HasColumnOrder(0);
        
        builder.Property(email => email.LidId).HasColumnOrder(1);

        builder.Property(email => email.Value).HasColumnOrder(2);
        
        builder.Property(email => email.Ordinal).HasColumnOrder(3);
    }
}