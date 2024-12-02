using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Tests.ArchitectureTests.Domain;

public class EntityTests : ArchitectureTestBase
{
    private readonly PredicateList _entities = Types
        .InAssembly(DomainAssembly)
        .That().Inherit(typeof(Entity<>));
    
    [Fact]
    public void Entities_ShouldBe_Sealed()
    {
        var conditionList = _entities
            .And().AreNotAbstract()
            .Should().BeSealed();
        
        conditionList.ShouldNotContainMatchingTypes();
    }

    [Fact]
    public void Entities_Should_HavePrivateSetters()
    {
        _entities.ShouldNot(HasNonPrivateSetter);
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        _entities.ShouldAll(HasPrivateParameterlessConstructor);
    }
}