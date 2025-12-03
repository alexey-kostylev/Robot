using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChampionRobot.Logic;
using ChampionRobot.Logic.Commands;

namespace ChampionRobot.UnitTests;

public partial class CommandTests
{
    [Theory]
    [InlineData(0, 0, Direction.EAST)] //crossing right border
    [InlineData(4, 0, Direction.WEST)] //crossing left border
    [InlineData(4, 4, Direction.WEST)] //crossing left border
    [InlineData(0, 4, Direction.NORTH)] //crossing TOP border
    [InlineData(4, 4, Direction.NORTH)] //crossing TOP border
    [InlineData(0, 0, Direction.SOUTH)] //crossing bottom border
    public void ShouldNotMove(int x, int y, Direction direction)
    {
        var position = new Logic.Position(x, y);
        _robot.HandleCommand(new PlaceCommand(position, direction));
        var result = _robot.HandleCommand(new MoveCommand());

        result.Sucess.Should().BeFalse();
        _robot._currentPosition.Should().Be(position);
    }

    [Theory]
    [InlineData(0, 0, Direction.NORTH, 0, 1)]
    [InlineData(0, 0, Direction.WEST, 1, 0)]
    [InlineData(2, 2, Direction.WEST, 3, 2)]
    public void ShouldMoveToPosition(int x, int y, Direction direction, int expectedX, int expectedY)
    {
        _robot.HandleCommand(new PlaceCommand(new Logic.Position(x, y), direction));
        var result = _robot.HandleCommand(new MoveCommand());

        result.Sucess.Should().BeTrue();

        _robot._currentPosition?.X.Should().Be(expectedX);
        _robot._currentPosition?.Y.Should().Be(expectedY);
    }
}
