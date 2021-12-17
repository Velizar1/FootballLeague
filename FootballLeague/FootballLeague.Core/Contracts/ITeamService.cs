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
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<TeamModel> GetTeamByIdAsync(Guid Id);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="matchModel"></param>
        /// <returns></returns>
        public Task<RepositoryResult> AddTeamAsync(TeamAddModel matchModel);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<RepositoryResult> RemoveTeamAsync(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="matchModel"></param>
        /// <returns></returns>
        public Task<RepositoryResult> UdpateTeamAsync(TeamEditModel matchModel);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <returns></returns>
        public List<TeamModel> AllTeams();

       
    }
}
