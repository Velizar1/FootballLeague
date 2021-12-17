using FootballLeague.Core.Commons;
using FootballLeague.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Contracts
{
    public interface IMatchService
    {
        /// <summary>
        /// Add match
        /// </summary>
        /// <param name="matchModel">Model of the match</param>
        /// <returns></returns>
        public Task<RepositoryResult> AddMatchAsync(MatchModel matchModel);

        /// <summary>
        /// Remove match
        /// </summary>
        /// <param name="id">Identificator of the match</param>
        /// <returns>RepositoryResult with a message and a boolean isSuccess</returns>
        public Task<RepositoryResult> RemoveMatchAsync(Guid id);

        /// <summary>
        /// Update match
        /// </summary>
        /// <param name="matchModel">Model of the match</param>
        /// <returns>RepositoryResult with a message and a boolean isSuccess</returns>
        public Task<RepositoryResult> UdpateMatchAsync(MatchModel matchModel);

        /// <summary>
        /// Get all matches
        /// </summary>
        /// <returns>Collection of matches</returns>
        public IEnumerable<MatchModel> AllMatches();

        /// <summary>
        /// Get a match by id
        /// </summary>
        /// <param name="id">Identificator of a match</param>
        /// <returns>Match by a given id</returns>
        public Task<MatchModel> GetMatchByIdAsync(Guid id);

    }
}
