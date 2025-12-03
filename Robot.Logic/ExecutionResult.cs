namespace ChampionRobot.Logic;

public record ExecutionResult(bool Sucess, string? Message)
{
    public static ExecutionResult Success() => new(true, null);

    public static ExecutionResult Success(string message) => new(true, message);

    public static ExecutionResult Fail(string reason) => new(false, reason);
}
