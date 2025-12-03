using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ChampionRobot.Logic;
public static class LogicRegistrationExtension
{
    public static IServiceCollection RegisterLogic(this IServiceCollection services)
        => services.AddTransient<Robot>();
}
