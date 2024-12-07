using DA.Anubis.Domain.BetaalmethodeAggregate;
using DA.Anubis.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.Anubis.Persistence.ModelConfiguration;

public sealed class IncassoBetaalmethodeConfiguration : IEntityTypeConfiguration<IncassoBetaalmethode>
{
    public void Configure(EntityTypeBuilder<IncassoBetaalmethode> builder)
    {
        builder.OwnsOne(incassoBetaalmethode => incassoBetaalmethode.Bankrekening, innerBuilder =>
        {
            innerBuilder.Property(bankrekening => bankrekening.Iban).HasConversion(new IbanConverter());
            innerBuilder.Property(bankrekening => bankrekening.Bic).HasMaxLength(11);
            innerBuilder.Property(bankrekening => bankrekening.TenNameVan).HasMaxLength(127);
        });

        builder.OwnsOne(incassoBetaalmethode => incassoBetaalmethode.IncassoMandaat, innerBuilder =>
        {
            innerBuilder.Property(mandaat => mandaat.Rekeningnummer).HasMaxLength(34);
            innerBuilder.Property(mandaat => mandaat.Type).HasConversion<int>();
            innerBuilder.Property(mandaat => mandaat.Tekendatum).HasColumnType("date");
            innerBuilder.Property(mandaat => mandaat.TekenInformatie).HasMaxLength(127);
        });
    }
}