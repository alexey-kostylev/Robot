using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChampionRobot.Logic;
using Microsoft.Extensions.Logging.Abstractions;

namespace ChampionRobot.UnitTests;
public partial class CommandTests
{
    private readonly Robot _robot = new(NullLogger<Robot>.Instance);
}
