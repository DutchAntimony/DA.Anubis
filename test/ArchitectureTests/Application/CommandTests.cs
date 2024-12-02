using DA.DDD.ApplicationLibrary.Messaging;

namespace DA.Anubis.Tests.ArchitectureTests.Application;

public class CommandTests : ArchitectureTestBase
{
    private readonly PredicateList _commands = Types
        .InAssembly(ApplicationAssembly)
        .That().ImplementInterface(typeof(ICommand))
        .Or().ImplementInterface(typeof(ICreateCommand<>));
    
    private readonly PredicateList _commandHandlers = Types
        .InAssembly(ApplicationAssembly)
        .That().ImplementInterface(typeof(ICommandHandler<>))
        .Or().ImplementInterface(typeof(ICreateCommandHandler<,>));

    [Fact]
    public void Commands_ShouldHave_NameEndingWith_Command()
    {
        var conditionList = _commands.Should().HaveNameEndingWith("Command");
        conditionList.ShouldNotContainMatchingTypes();
    }
    
    [Fact]
    public void CommandHandlers_ShouldHave_NameEndingWith_CommandHandler()
    {
        var conditionList = _commandHandlers.Should().HaveNameEndingWith("Handler");
        conditionList.ShouldNotContainMatchingTypes();
    }
    
    [Fact]
    public void CommandHandlers_ShouldBe_Internal()
    {
        var conditionList = _commandHandlers.Should().NotBePublic();
        conditionList.ShouldNotContainMatchingTypes();
    }
    
    [Fact]
    public void CommandHandlers_ShouldBe_Sealed()
    {
        var conditionList = _commandHandlers.Should().BeSealed();
        conditionList.ShouldNotContainMatchingTypes();
    }
}