using FootballLeague.Core.Contracts;
using FootballLeague.Core.Contracts.Impl;
using FootballLeague.Core.Repositories;
using FootballLeague.Core.Repositories.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IScoreService, ScoreService>();
        }
    }
}
