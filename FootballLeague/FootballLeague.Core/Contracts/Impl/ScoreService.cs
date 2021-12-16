﻿using FootballLeague.Core.Models;
using FootballLeague.Core.Repositories;
using FootballLeague.DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Contracts.Impl
{
    public class ScoreService : IScoreService
    {
        private readonly IRepository repo;


        public ScoreService(IRepository _repo)
        {
            this.repo = _repo;
        }
        public async Task CalculateScore<T>(Match<T> match, T VisitingTeamId, T HostingTeamId, bool revertScore = false)
        {
            if (match == null)
            {
                match = await repo.All<Match<T>>()
               .Include(x => x.HostingTeam)
               .Include(x => x.VisitingTeam)
               .Where(x => x.VisitingTeamId.Equals(VisitingTeamId) &&
                           x.HostingTeamId.Equals(HostingTeamId))
               .FirstOrDefaultAsync();
            }

            if (match != null)
            {
               Team<T> teamModel;
                try
                {
                    if (match.HostingTeamScore > match.VisitingTeamScore)
                    {
                        teamModel = match.HostingTeam;
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -3 : 3);
                    }
                    else if (match.HostingTeamScore < match.VisitingTeamScore)
                    {
                        teamModel = match.VisitingTeam;
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -3 : 3);
                    }
                    else
                    {
                        teamModel = match.VisitingTeam;
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -1 : 1);
                        repo.Update<Team<T>>(teamModel);
                        
                        teamModel = match.HostingTeam;
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -1 : 1);

                    }
                    repo.Update<Team<T>>(teamModel);
                    await repo.SavechangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}