using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChampionRobot.Logic.Commands;
public static class CommandParser
{
    /// <summary>
    /// Parse command
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static CommandBase ParseCommand(string payload)
    {
        if (payload.StartsWith("place", StringComparison.InvariantCultureIgnoreCase))
        {
            return ParsePlaceCommand(payload);
        }

        if (payload.StartsWith("move", StringComparison.InvariantCultureIgnoreCase))
        {
            return new MoveCommand();
        }

        if (payload.StartsWith("report", StringComparison.InvariantCultureIgnoreCase))
        {
            return new ReportCommand();
        }

        if (payload.StartsWith("left", StringComparison.InvariantCultureIgnoreCase))
        {
            return new LeftCommand();
        }

        if (payload.StartsWith("right", StringComparison.InvariantCultureIgnoreCase))
        {
            return new RightCommand();
        }

        throw new InvalidOperationException($"Command is invalid: {payload}");
    }

    private static PlaceCommand ParsePlaceCommand(string payload)
    {
        const string PlaceCommandPattern = @"^PLACE\s+(\d+),(\d+),([A-Z]+)$";
        var regex = new Regex(PlaceCommandPattern, RegexOptions.IgnoreCase);
        Match match = regex.Match(payload);

        if (!match.Success)
        {
            throw new InvalidOperationException($"The input is not valid: {payload}");
        }

        var xCoordinateString = match.Groups[1].Value;
        var yCoordinateString = match.Groups[2].Value;
        var directionString = match.Groups[3].Value;

        // Convert coordinates to integers
        int x = int.Parse(xCoordinateString);
        int y = int.Parse(yCoordinateString);

        if (!Enum.TryParse<Direction>(directionString, true, out var direction))
        {
            throw new InvalidOperationException($"Invalid direction: {directionString}");
        }

        return new PlaceCommand(new Position(x, y), direction);
    }
}
