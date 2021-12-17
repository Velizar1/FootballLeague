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
        /// Calculate team points
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="match">Match for the two teams</param>
        /// <param name="VisitingTeamId">VisitingTeam Id</param>
        /// <param name="HostingTeamId">HostingTeam Id</param>
        /// <param name="revertScore">Show if the calculation must be normal or reverted</param>
        /// <returns></returns>
        public Task CalculateScore(Match match, Guid VisitingTeamId, Guid HostingTeamId, bool revertScore = false);
    }
}
