using FootballLeague.Core.Constants;
using FootballLeague.Core.Models;
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
            repo = _repo;
        }

        public async Task CalculatePoints(Match match, Guid VisitingTeamId, Guid HostingTeamId, bool revertScore = false)
        {
            if (match == null)
            {
                match = await repo.All<Match>()
               .Include(x => x.HostingTeam)
               .Include(x => x.VisitingTeam)
               .Where(x => x.VisitingTeamId.Equals(VisitingTeamId) &&
                           x.HostingTeamId.Equals(HostingTeamId))
               .FirstOrDefaultAsync();
            }

            if (match != null)
            {
                Team teamModel;

                if (match.HostingTeamScore > match.VisitingTeamScore)
                {
                    teamModel = match.HostingTeam;
                    teamModel.TeamPoints = teamModel.TeamPoints + (revertScore ? -3 : 3);
                }
                else if (match.HostingTeamScore < match.VisitingTeamScore)
                {
                    teamModel = match.VisitingTeam;
                    teamModel.TeamPoints = teamModel.TeamPoints + (revertScore ? -3 : 3);
                }
                else
                {
                    teamModel = match.VisitingTeam;
                    teamModel.TeamPoints = teamModel.TeamPoints + (revertScore ? -1 : 1);
                    repo.Update(teamModel);

                    teamModel = match.HostingTeam;
                    teamModel.TeamPoints = teamModel.TeamPoints + (revertScore ? -1 : 1);

                }

                repo.Update(teamModel);
                await repo.SavechangesAsync();
            }
            else
            {
                throw new ArgumentException(ExceptionConstants.Message.NotFound);
            }
        }
    }
}
