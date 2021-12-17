using FootballLeague.DataAccess.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Contracts
{
    public interface IScoreService
    {
        /// <summary>
        /// Calculate teams points
        /// </summary>
        /// <param name="match">Match of the two teams</param>
        /// <param name="VisitingTeamId">VisitingTeam Id</param>
        /// <param name="HostingTeamId">HostingTeam Id</param>
        /// <param name="revertPoints">Revert each teams points</param>

        public Task CalculatePoints(Match match, Guid VisitingTeamId, Guid HostingTeamId, bool revertPoints = false);
    }
}
