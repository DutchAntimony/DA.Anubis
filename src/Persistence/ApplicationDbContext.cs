using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.DDD.CoreLibrary.ServiceDefinitions;
using DA.DDD.PersistenceLibrary.EFCore;
using DA.DDD.PersistenceLibrary.EFCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DA.Anubis.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
    IMediator mediator, IDateTimeProvider dateTimeProvider, ICurrentUserProvider currentUserProvider) 
    : DbContextBase(options, mediator, dateTimeProvider, currentUserProvider, typeof(ApplicationDbContext).Assembly)
{
    protected override void ConfigureEntityKeysAndOtherConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .AddEntityKeyConvention<LidId>()
            .AddEntityKeyConvention<AdresId>()
            .AddEntityKeyConvention<BetaalmethodeId>()
            .AddEntityKeyConvention<MutatieId>();
    }
}