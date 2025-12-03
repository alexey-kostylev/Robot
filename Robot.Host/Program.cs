// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using System.Reflection;
using ChampionRobot.Logic;
using ChampionRobot.Logic.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;


Microsoft.Extensions.Logging.ILogger? logger = null;
CancellationTokenSource _cts = new CancellationTokenSource();
try
{
    var services = new ServiceCollection();

    var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new InvalidOperationException("Can not get base directory");

    var configuration = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .Build();

    SetupLogging(services, configuration);
    services.RegisterLogic();

    var provider = services.BuildServiceProvider();
    var robot = provider.GetRequiredService<Robot>();

    var logFactory = provider.GetRequiredService<ILoggerFactory>();

    logger = logFactory.CreateLogger("main");

    Option<string> fileOption = new("--file")
    {
        Description = "Command file to execute."
    };

    var rootCommand = new RootCommand("Sample");
    rootCommand.Add(fileOption);

    var parsed = rootCommand.Parse(args);

    if (parsed.Errors.Count == 0 && parsed.GetValue(fileOption) is string commandFile)
    {
        Console.WriteLine($"Total arguments received: {args.Length}");
        Console.WriteLine("Reading command file {0}", commandFile);

        var lines = File.ReadAllLines(Path.Combine(basePath, commandFile));
        Console.WriteLine($"{commandFile} has {lines.Length} commands");

        List<CommandBase> parsedCommands = new();
        foreach (var commandLine in lines)
        {
            var command = CommandParser.ParseCommand(commandLine);
            if (command != null)
            {
                parsedCommands.Add(command);
            }
            else
            {
                Console.WriteLine("Command is not parsed: {0}", commandLine);
            }
        }

        if (!parsedCommands.Any())
        {
            return;
        }

        foreach (var command in parsedCommands)
        {
            ExecuteCommand(robot, command);
        }
    }
    else
    {
        while (!_cts.IsCancellationRequested)
        {
            var command = DisplayPrompt();
            if (command is not null)
            {
                ExecuteCommand(robot, command);
            }
        }
    }
}
catch (Exception e)
{
    logger?.LogError(e, "Unknown error");
}
finally
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("Execution has been completed. Press any key to exit...");
    Console.ResetColor();
    Console.ReadKey();
}

static void ExecuteCommand(Robot robot, CommandBase command)
{
    var result = robot.HandleCommand(command);
    Console.ForegroundColor = result.Sucess ? ConsoleColor.Green : ConsoleColor.Magenta;
    Console.WriteLine("{0}", result.Message);
    Console.ResetColor();
}

CommandBase? DisplayPrompt()
{
    CommandBase? command = null;
    while (command is null)
    {
        Console.WriteLine($"Enter command or press Enter to exit.");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(">");
        Console.ResetColor();
        var commandInput = Console.ReadLine();
        if (string.IsNullOrEmpty(commandInput))
        {
            _cts.Cancel();
            return null;
        }
        try
        {
            command = CommandParser.ParseCommand(commandInput ?? "");
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"** Error: {e}");
            Console.ResetColor();
        }
    }

    return command;
}

static IServiceCollection SetupLogging(IServiceCollection services, IConfiguration configuration)
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    services.AddLogging(builder =>
    {
        builder.AddSerilog();
    });

    return services;
}

