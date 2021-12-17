using FootballLeague.Core.Commons;
using FootballLeague.Core.Models;
using FootballLeague.DataAccess.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Contracts
{
    public interface ITeamService
    {
        /// <summary>
        /// Get a team by id
        /// </summary>
        /// <param name="Id">Identificator of a team</param>
        /// <returns></returns>
        public Task<TeamModel> GetTeamByIdAsync(Guid Id);

        /// <summary>
        /// Add a team record
        /// </summary>
        /// <param name="matchModel">Model of a team</param>
        /// <returns></returns>
        public Task<RepositoryResult> AddTeamAsync(TeamModel matchModel);

        /// <summary>
        /// Remove a team record
        /// </summary>
        /// <param name="id">Indentificator of a team</param>
        /// <returns></returns>
        public Task<RepositoryResult> RemoveTeamAsync(Guid id);

        /// <summary>
        /// Update a team
        /// </summary>
        /// <param name="matchModel">Model of a team</param>
        /// <returns></returns>
        public Task<RepositoryResult> UdpateTeamAsync(TeamModel matchModel);

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeamModel> AllTeams();


    }
}
