using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChampionRobot.Logic.Commands;
using ChampionRobot.Logic;

namespace ChampionRobot.UnitTests;
public class CommandParserTests
{
    [Fact]
    public void ParseCommand_WithPlaceCommand_ReturnsPlaceCommand()
    {
        // Arrange
        string payload = "PLACE 1,2,NORTH";

        // Act
        var result = CommandParser.ParseCommand(payload);

        // Assert
        result.Should().BeOfType<PlaceCommand>();
        var placeCommand = result as PlaceCommand;
        placeCommand?.CommandType.Should().Be(CommandType.Place);
        placeCommand?.Position.X.Should().Be(1);
        placeCommand?.Position.Y.Should().Be(2);
        placeCommand?.Direction.Should().Be(Direction.NORTH);
    }

    [Fact]
    public void ParseCommand_WithPlaceCommand_ReturnsInvalid()
    {
        // Arrange
        string payload = "PLACE 0 0 NORTH";
        // ACT & Assert
        Action action = () => CommandParser.ParseCommand(payload);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("The input is not valid:*");
    }

    [Theory]
    [InlineData("MOVE")]
    [InlineData("move")]
    [InlineData("MoVE")]
    public void ParseCommand_WithMoveCommand_ReturnsMoveCommand(string commandInput)
    {
        // Act
        var result = CommandParser.ParseCommand(commandInput);

        // Assert
        result.Should().BeOfType<MoveCommand>();
        result.CommandType.Should().Be(CommandType.Move);
    }

    // Add more tests for other command types

    [Fact]
    public void ParseCommand_WithInvalidInput_ThrowsInvalidOperationException()
    {
        // Arrange
        string payload = "INVALID";

        // Act & Assert
        Action action = () => CommandParser.ParseCommand(payload);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage($"Command is invalid: INVALID");
    }
}
