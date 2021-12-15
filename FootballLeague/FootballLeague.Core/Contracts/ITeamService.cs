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
        public Task<TeamModel<T>> GetTeamByIdAsync<T>(T Id);

        public Task<RepositoryResult> AddTeamAsync<T>(TeamModel<T> matchModel);

        public Task<RepositoryResult> AddHostedMatchAsync<T>(T TeamId, MatchModel<T> matchModel);
        public Task<RepositoryResult> AddVisitedMatchAsync<T>(T TeamId, MatchModel<T> matchModel);
        public Task<RepositoryResult> RemoveTeamAsync<T>(T id);

        public Task<RepositoryResult> UdpateTeamAsync<T>(TeamModel<T> matchModel);

        public List<TeamModel<T>> AllTeams<T>();

       
    }
}
