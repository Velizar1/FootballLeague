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
        public Task<RepositoryResult> AddMatchAsync<T>(MatchModel<T> matchModel);

        public Task<RepositoryResult> RemoveMatchAsync<T>(T id);

        public Task<RepositoryResult> UdpateMatchAsync<T>(MatchModel<T> matchModel);

        public List<MatchModel<T>> AllMatches<T>();

        public Task<MatchModel<T>> GetMatchByIdAsync<T>(T id);
        public Task<MatchModel<T>> GetMatchByTeamsAsync<T>(T visitngTeamId, T hostingTeamId);
    }
}
