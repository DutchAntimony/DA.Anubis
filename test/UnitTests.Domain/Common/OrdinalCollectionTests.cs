using DA.Anubis.Domain.Common;
using DA.DDD.CoreLibrary.Entities;
using DA.Results.Shouldly;

namespace DA.Anubis.Tests.UnitTests.Application.Common;

public class OrdinalCollectionTests
{
    private readonly List<OrdinalTestEntity> _entities = [];
    private readonly IOrdinalCollection<OrdinalTestEntity, OrdinalEntityTestId> _ordinalCollection;
    
    public OrdinalCollectionTests()
    {
        _ordinalCollection = new OrdinalCollection<OrdinalTestEntity, OrdinalEntityTestId>(_entities);
    }
    
    [Fact]
    public void Append_Should_AddEntityWithCorrectOrdinal()
    {
        _ordinalCollection.Count.ShouldBe(0);
        _ordinalCollection.Append(new OrdinalTestEntity(1));
        _ordinalCollection.Count.ShouldBe(1);
        _entities.Count.ShouldBe(1);
        _entities.Single().Ordinal.ShouldBe(1);
        TestCleanup();
    }

    [Fact]
    public void AppendMany_Should_AddMultipleEntityWithCorrectOrdinal()
    {
        _ordinalCollection.Count.ShouldBe(0);
        _ordinalCollection.AppendMany(
            new OrdinalTestEntity(1),
            new OrdinalTestEntity(2),
            new OrdinalTestEntity(3)
        );
        _ordinalCollection.Count.ShouldBe(3);
        _entities.Count.ShouldBe(3);
        _entities.ForEach(e => e.Ordinal.ShouldBe(e.Index));
        TestCleanup();
    }

    [Fact]
    public void MoveEntityToTheBeginning_Should_MoveEntityUp_FromKey()
    {
        _ordinalCollection.Count.ShouldBe(0);
        var toMove = Insert5ElementsAndReturnMiddle();
        _ordinalCollection.MoveEntityToTheBeginning(toMove.Id).ShouldBeSuccess();
        VerifyExpectedOrderOfIndices([3,1,2,4,5]);
    }

    [Fact]
    public void MoveEntityUp_Should_MoveEntityUp_FromKey()
    {
        _ordinalCollection.Count.ShouldBe(0);
        var toMove = Insert5ElementsAndReturnMiddle();
        _ordinalCollection.MoveEntityUp(toMove.Id).ShouldBeSuccess();
        VerifyExpectedOrderOfIndices([1,3,2,4,5]);
    }
    
    [Fact]
    public void MoveEntityDown_Should_MoveEntityDown_FromKey()
    {
        _ordinalCollection.Count.ShouldBe(0);
        var toMove = Insert5ElementsAndReturnMiddle();
        _ordinalCollection.MoveEntityDown(toMove.Id).ShouldBeSuccess();
        VerifyExpectedOrderOfIndices([1,2,4,3,5]);
    }
    
    [Fact]
    public void MoveEntityToTheEnd_Should_MoveEntityDown_FromKey()
    {
        _ordinalCollection.Count.ShouldBe(0);
        var toMove = Insert5ElementsAndReturnMiddle();
        _ordinalCollection.MoveEntityToTheEnd(toMove.Id).ShouldBeSuccess();
        VerifyExpectedOrderOfIndices([1,2,4,5,3]);
    }
    
    private OrdinalTestEntity Insert5ElementsAndReturnMiddle()
    {
        var middle = new OrdinalTestEntity(3);
        _ordinalCollection.AppendMany(
            new OrdinalTestEntity(1),
            new OrdinalTestEntity(2),
            middle,
            new OrdinalTestEntity(4),
            new OrdinalTestEntity(5)
        );
        return middle;
    }

    private void VerifyExpectedOrderOfIndices(int[] expected)
    {
        _entities.ForEach(e => e.Index.ShouldBe(expected[e.Ordinal-1]));
    }
    
    private void TestCleanup()
    {
        _entities.Clear();
        _ordinalCollection.Count().ShouldBe(0);
    }
    
    private record struct OrdinalEntityTestId : IEntityKey
    {
        public Guid Value { get; init; }
    }

    private class OrdinalTestEntity(int index) : OrdinalEntity<OrdinalEntityTestId>
    {
        public int Index { get; } = index;
    }
}