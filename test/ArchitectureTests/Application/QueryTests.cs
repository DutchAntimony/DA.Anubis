using DA.DDD.ApplicationLibrary.Messaging;

namespace DA.Anubis.Tests.ArchitectureTests.Application;

public class QueryTests : ArchitectureTestBase
{
    private readonly PredicateList _queries = Types
        .InAssembly(ApplicationAssembly)
        .That().ImplementInterface(typeof(IQuery<>));
    
    private readonly PredicateList _queryHandlers = Types
        .InAssembly(ApplicationAssembly)
        .That().ImplementInterface(typeof(IQueryHandler<,>))
        .Or().ImplementInterface(typeof(IPagedQueryHandler<,>));
    
    [Fact]
    public void Queries_ShouldHave_NameEndingWith_Query()
    {
        var conditionList = _queries.Should().HaveNameEndingWith("Query");
        conditionList.ShouldNotContainMatchingTypes();
    }
    
    [Fact]
    public void QueryHandlers_ShouldHave_NameEndingWith_QueryHandler()
    {
        var conditionList = _queryHandlers.Should().HaveNameEndingWith("Handler");
        conditionList.ShouldNotContainMatchingTypes();
    }
    
    [Fact]
    public void QueryHandlers_ShouldBe_Internal()
    {
        var conditionList = _queryHandlers.Should().NotBePublic();
        conditionList.ShouldNotContainMatchingTypes();
    }
    
    [Fact]
    public void QueryHandlers_ShouldBe_Sealed()
    {
        var conditionList = _queryHandlers.Should().BeSealed();
        conditionList.ShouldNotContainMatchingTypes();
    }
}