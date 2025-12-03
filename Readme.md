# Champion robot test

## How to run the project

Goto the project directory and run the following command  from command line:
```bash 
dotnet build 
```

When the build is done, run the following command
```bash
dotnet run --project Robot.host/ChampionRobot.Host.csproj --file commands.txt
```

To run a standalone console application you need to go to "Robot.Host\bin\Debug\net8.0" directory and run the following command:
```bash
ChampionRobot.Host.exe --file commands.txt
```

It will run the project using the commands.txt file located in the project root directory.

To run it interactively, run the following command omitting '--file' argument:
```bash
ChampionRobot.Host.exe
```


## Project structure
The project has three main directories: 
- Robot.Host
- Robot.Logic
- Robot.UnitTests

Robot.Host is the console application contains the code for the host application, which is responsible for running the robot and handling commands.
Console supports reading commands file from the argument in the format: '--file {path}'. Where path is a local path relative to the bin directory.
Without arguments console will work in interactive mode reading commands from the screen.

Robot.Logic contains the code for the robot's logic, including the ChampionRobot.Logic project, which defines the robot's behavior.

Robot.UnitTests contains the unit tests for the robot's logic.

Robot is the root directory of the project, and contains the Readme.md file, which provides instructions on how to run the project.
The project uses the .NET Core framework, and the dotnet build command is used to build the project. After building, the project can be run using the dotnet run command, with the --project flag specifying the project to run, and the --file flag specifying the file containing the commands to execute.