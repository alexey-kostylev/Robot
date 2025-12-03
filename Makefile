all: build test

build:
	dotnet build

test:
	dotnet test
	
run: build
	dotnet run --project Robot.host/ChampionRobot.Host.csproj
	
file: build
	dotnet run --project Robot.host/ChampionRobot.Host.csproj --file commands.txt