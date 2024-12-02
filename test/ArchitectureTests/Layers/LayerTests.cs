namespace DA.Anubis.Tests.ArchitectureTests.Layers;

public class LayerTests : ArchitectureTestBase
{
    [Fact]
    public void DomainLayer_Should_NotHaveIncorrectDependencies()
    {
        DomainAssembly.ShouldNotHaveDependenciesOn(InfrastructureAssembly, ApplicationAssembly, PersistenceAssembly);
    }

    [Fact]
    public void InfrastructureLayer_Should_NotHaveIncorrectDependencies()
    {
        InfrastructureAssembly.ShouldNotHaveDependenciesOn(ApplicationAssembly, PersistenceAssembly);
    }

    [Fact]
    public void PersistenceLayer_Should_NotHaveIncorrectDependencies()
    {
        PersistenceAssembly.ShouldNotHaveDependenciesOn(ApplicationAssembly, InfrastructureAssembly);
    }

    [Fact]
    public void UiLayer_Should_NotHaveIncorrectDependencies()
    {
        Types.InAssembly(UiConsoleAssembly)
            .That().DoNotHaveName(nameof(Program))
            .And().DoNotResideInNamespaceContaining("Installers")
            .ShouldNotHaveDependenciesOn(InfrastructureAssembly, PersistenceAssembly);
    }
}