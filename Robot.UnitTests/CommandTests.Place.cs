using ChampionRobot.Logic;
using ChampionRobot.Logic.Commands;

namespace ChampionRobot.UnitTests;
public partial class CommandTests
{
    [Fact]
    public void ShouldPlace()
    {
        var result = _robot.HandleCommand(new PlaceCommand(new Position(1, 1), Direction.SOUTH));

        result.Sucess.Should().BeTrue();

        _robot._currentDirection.Should().Be(Direction.SOUTH);
        _robot._currentPosition.Should().Be(new Position(1, 1));
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void ShouldNotPlace(int x, int y)
    {
        var position = new Position(x, y);
        var result = _robot.HandleCommand(new PlaceCommand(position, Direction.NORTH));

        result.Sucess.Should().BeFalse();

        _robot._currentDirection.Should().BeNull();
        _robot._currentPosition.Should().BeNull();
    }
}
