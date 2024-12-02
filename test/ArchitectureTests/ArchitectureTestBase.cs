using System.Reflection;
using DA.Anubis.Application;
using DA.Anubis.Domain;
using DA.Anubis.Infrastructure;
using DA.Anubis.Persistence;

namespace DA.Anubis.Tests.ArchitectureTests;

public class ArchitectureTestBase
{
    protected static readonly Assembly ApplicationAssembly = typeof(IApplicationMarker).Assembly;
    protected static readonly Assembly DomainAssembly = typeof(IDomainMarker).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(IInfrastructureMarker).Assembly;
    protected static readonly Assembly PersistenceAssembly = typeof(IPersistenceMarker).Assembly;
    protected static readonly Assembly UiConsoleAssembly = typeof(Program).Assembly;
 
    ///<summary>
    /// Function to check if a type has only private setters.
    /// </summary>
    protected static bool HasNonPrivateSetter(Type type)
    {
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            if (property.CanWrite && 
                property.SetMethod != null &&
                !property.SetMethod.IsPrivate && 
                !property.SetMethod.IsFamily && 
                !property.SetMethod.IsFamilyAndAssembly)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Function to check if a type has a private parameterless constructor
    /// </summary>
    protected static bool HasPrivateParameterlessConstructor(Type type)
    {
        var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
        return constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0);
    }
}