all: build test

build:
	dotnet build

test:
	dotnet test
	
file: build
	dotnet run --project Robot.host/ChampionRobot.Host.csproj --file commands.txt