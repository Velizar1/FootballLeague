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
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="matchModel"></param>
        /// <returns></returns>
        public Task<RepositoryResult> AddMatchAsync(MatchModel matchModel);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<RepositoryResult> RemoveMatchAsync(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="matchModel"></param>
        /// <returns></returns>
        public Task<RepositoryResult> UdpateMatchAsync(MatchModel matchModel);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <returns></returns>
        public IEnumerable<MatchModel> AllMatches();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MatchModel> GetMatchByIdAsync(Guid id);
       
    }
}
