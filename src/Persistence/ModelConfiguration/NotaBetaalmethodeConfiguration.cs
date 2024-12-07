using DA.Anubis.Domain.BetaalmethodeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class NotaBetaalmethodeConfiguration : IEntityTypeConfiguration<NotaBetaalmethode>
{
    public void Configure(EntityTypeBuilder<NotaBetaalmethode> builder)
    {
        builder.Property(notaBetaalmethode => notaBetaalmethode.PrefereerEmail)
            .HasColumnName("Nota_PrefereerEmail");
    }
}