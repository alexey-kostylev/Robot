namespace ChampionRobot.Logic.Commands;

public record PlaceCommand(Position Position, Direction Direction) : CommandBase(CommandType.Place)
{

}

public record MoveCommand() : CommandBase(CommandType.Move)
{

}

public record LeftCommand() : CommandBase(CommandType.Left)
{

}

public record RightCommand() : CommandBase(CommandType.Right)
{

}

public record ReportCommand() : CommandBase(CommandType.Report)
{

}
