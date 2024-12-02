using System.Reflection;

namespace DA.Anubis.Tests.ArchitectureTests;

internal static class NetArchTestExtensions
{
    public static void ShouldNotContainMatchingTypes(this ConditionList conditions)
    {
        var types = conditions.GetResult();
        types.FailingTypeNames.ShouldBeNull();
        types.IsSuccessful.ShouldBeTrue();
    }

    public static void ShouldAll(this PredicateList collection, Predicate<Type> predicate)
    {
        var types = collection.GetTypes();
        var failingTypeNames = types.Where(t => !predicate(t)).Select(t => t.Name).ToList();
        failingTypeNames.ShouldBeEmpty();
    }
    
    public static void ShouldNot(this PredicateList collection, Predicate<Type> predicate)
    {
        var types = collection.GetTypes();
        var failingTypeNames = types.Where(t => predicate(t)).Select(t => t.Name).ToList();
        failingTypeNames.ShouldBeEmpty();
    }

    public static void ShouldNotHaveDependenciesOn(this Assembly check, params Assembly[] dependencies)
    {
        var types = 
            Types.InAssembly(check)
            .Should().NotHaveDependencyOnAny(dependencies.Select(d => d.GetName().Name).ToArray())
            .GetResult();
        types.FailingTypeNames.ShouldBeNull();
        types.IsSuccessful.ShouldBeTrue();
    }

    public static void ShouldNotHaveDependenciesOn(this PredicateList collection, params Assembly[] dependencies)
    {
        var types = collection
            .Should().NotHaveDependencyOnAny(dependencies.Select(d => d.GetName().Name).ToArray())
            .GetResult();
        
        types.FailingTypeNames.ShouldBeNull();
        types.IsSuccessful.ShouldBeTrue();
    }
}