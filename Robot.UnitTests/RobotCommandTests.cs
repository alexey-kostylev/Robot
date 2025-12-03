using ChampionRobot.Logic;
using ChampionRobot.Logic.Commands;
using Microsoft.Extensions.Logging.Abstractions;

namespace ChampionRobot.UnitTests;

public class RobotCommandTests
{
    private readonly Robot _robot = new(NullLogger<Robot>.Instance);

    [Theory]
    [InlineData("move")]
    [InlineData("left")]
    [InlineData("right")]
    public void RobotIsNotPlacedShouldFail(string commandInput)
    {
        var result = _robot.HandleCommand(commandInput switch
        {
            "move" => new MoveCommand(),
            "left" => new LeftCommand(),
            "right" => new RightCommand(),
            _ => throw new NotImplementedException(),
        });

        result.Sucess.Should().BeFalse();
        result.Message.Should().Be("Robot must be positioned first");
    }

    [Theory]
    [InlineData(
        """
        PLACE 0,0,NORTH
        MOVE
        REPORT
        """,
        "0,1,NORTH")]
    public void Test1(string input, string expected)
    {
        ExecutionResult? lastResult = null;
        foreach (var commandInput in GetCommands(input))
        {
            var command = CommandParser.ParseCommand(commandInput);
            lastResult = _robot.HandleCommand(command);
        }
        var actualValue = lastResult?.Message;

        actualValue.Should().Be(expected);
    }

    [Theory]
    [InlineData(
        """
        PLACE 0,0,NORTH
        LEFT
        REPORT
        """,
        "0,0,WEST")]
    public void Test2(string input, string expected)
    {
        ExecutionResult? lastResult = null;
        foreach (var commandInput in GetCommands(input))
        {
            var command = CommandParser.ParseCommand(commandInput);
            lastResult = _robot.HandleCommand(command);
        }
        var actualValue = lastResult?.Message;

        actualValue.Should().Be(expected);
    }

    private static ICollection<string> GetCommands(string input)
    {
        return input.Split(
            new[] { '\r', '\n' },
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
    }
}
