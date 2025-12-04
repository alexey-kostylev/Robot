using ChampionRobot.Logic.Commands;
using Microsoft.Extensions.Logging;

namespace ChampionRobot.Logic;

public class Robot(ILogger<Robot> _logger)
{
    private const int MAP_SIZE = 5;
    private readonly ILogger<Robot> _logger = _logger;
    internal Position? _currentPosition;
    internal Direction? _currentDirection;

    private readonly List<(CommandBase command, DateTimeOffset timeStamp, ExecutionResult result)> _commandLog = [];

    public ExecutionResult HandleCommand(CommandBase command)
    {
        _logger.LogTrace("Handling command: {Command}", command);
        try
        {
            var executionResult = command.CommandType switch
            {
                CommandType.Place => HandlePlaceCommand((PlaceCommand)command),
                CommandType.Move => HandleMoveCommand((MoveCommand)command),
                CommandType.Left => HandleLeftCommand((LeftCommand)command),
                CommandType.Right => HandleRightCommand((RightCommand)command),
                CommandType.Report => Report(),
                _ => throw new NotImplementedException(),
            };
            LogCommand(command, executionResult);
            _logger.LogInformation("Command has been successfully processed: {Command}", command);
            return executionResult;
        }
        catch(Exception e)
        {
            var executionResult = ExecutionResult.Fail(e.Message);
            LogCommand(command, executionResult);
            _logger.LogError(e, "Error processing command");
            return executionResult;
        }
        finally
        {
            _logger.LogTrace("State: [{X},{Y}] {Direction}", _currentPosition?.X, _currentPosition?.Y, _currentDirection);
        }
    }

    public ExecutionResult Report()
    {
        var message = IsPositioned() ?
            $"{_currentPosition?.X},{_currentPosition?.Y},{_currentDirection?.ToString().ToUpper()}":
            "Robot is not positioned";

        return ExecutionResult.Success(message);
    }

    private ExecutionResult HandlePlaceCommand(PlaceCommand command)
    {
        var checkResult = CheckPosition(command.Position);
        if (!checkResult.Sucess)
        {
            return checkResult;
        }

        _currentPosition = command.Position;
        _currentDirection = command.Direction;

        return ExecutionResult.Success($"Set position to: {_currentPosition} and direction to: {_currentDirection}");
    }

    private bool IsPositioned()
        => _currentPosition != null;

    private static ExecutionResult CreateRobotIsNotPositionedResult()
        => ExecutionResult.Fail("Robot must be positioned first");

    private ExecutionResult HandleLeftCommand(LeftCommand command)
        => Rotate(RotateDirection.Left);

    private ExecutionResult HandleRightCommand(RightCommand command)
        => Rotate(RotateDirection.Right);

    private ExecutionResult Rotate(RotateDirection rotateDirection)
    {
        if (!IsPositioned())
        {
            return CreateRobotIsNotPositionedResult();
        }

        if (_currentDirection == null)
        {
            throw new InvalidOperationException("Current direction is not set");
        }

        _currentDirection = rotateDirection switch
        {
            RotateDirection.Left => RotateLeft(_currentDirection.Value),
            RotateDirection.Right => RotateRight(_currentDirection.Value),
            _ => throw new NotImplementedException($"Invalid rotate direction: {rotateDirection}")
        };

        return ExecutionResult.Success($"Set a new direction: {_currentDirection}");
    }

    private static Direction RotateLeft(Direction direction)
    {
        return direction switch
        {
            Direction.NORTH => Direction.WEST,
            Direction.SOUTH => Direction.EAST,
            Direction.EAST => Direction.NORTH,
            Direction.WEST => Direction.SOUTH,
            _ => throw new InvalidOperationException($"Invalid direction: {direction}")
        };
    }

    private static Direction RotateRight(Direction direction)
    {
        return direction switch
        {
            Direction.NORTH => Direction.EAST,
            Direction.EAST => Direction.SOUTH,
            Direction.SOUTH => Direction.WEST,
            Direction.WEST => Direction.NORTH,
            _ => throw new InvalidOperationException($"Invalid direction: {direction}")
        };
    }

    private ExecutionResult HandleMoveCommand(MoveCommand command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (!IsPositioned())
        {
            return CreateRobotIsNotPositionedResult();
        }

        if (_currentDirection == null)
        {
            throw new InvalidOperationException("Current direction is not set");
        }

        var step = _currentDirection.Value switch
        {
            Direction.NORTH => (0, 1),
            Direction.SOUTH => (0, -1),
            Direction.EAST => (-1, 0),
            Direction.WEST => (1, 0),
            _ => throw new NotImplementedException($"Invalid direction: {_currentDirection}")
        };

        (int x, int y) = step;

        // _currentPosition is already checked for null
        var newPosition = _currentPosition! with
        {
            X = _currentPosition.X + x,
            Y = _currentPosition.Y + y,
        };

        var checkResult = CheckPosition(newPosition);
        if (!checkResult.Sucess)
        {
            return checkResult;
        }

        _currentPosition = newPosition;

        return ExecutionResult.Success($"Moved to a new position: {newPosition}");
    }

    private void LogCommand(CommandBase command, ExecutionResult executionResult)
        => _commandLog.Add((command, DateTimeOffset.UtcNow, executionResult));

    private static ExecutionResult CheckPosition(Position position)
    {
        if (position.X < 0)
        {
            return ExecutionResult.Fail("X must be positive or 0");
        }

        if (position.Y < 0)
        {
            return ExecutionResult.Fail("Y must be positive or 0");
        }

        if (position.X >= MAP_SIZE)
        {
            return ExecutionResult.Fail($"X must not exceed map size {MAP_SIZE}");
        }

        if (position.Y >= MAP_SIZE)
        {
            return ExecutionResult.Fail($"Y must not exceed map size {MAP_SIZE}");
        }
        return ExecutionResult.Success();
    }
}
