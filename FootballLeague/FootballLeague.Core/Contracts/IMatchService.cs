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
        public Task<RepositoryResult> AddMatchAsync<T>(MatchModelAdd<T> matchModel);

        public Task<RepositoryResult> RemoveMatchAsync<T>(T id);

        public Task<RepositoryResult> UdpateMatchAsync<T>(MatchModelEdit<T> matchModel);

        public List<MatchModelEdit<T>> AllMatches<T>();

        public Task<MatchModelEdit<T>> GetMatchByIdAsync<T>(T id);
        public Task<MatchModelEdit<T>> GetMatchByTeamsAsync<T>(T visitngTeamId, T hostingTeamId);
    }
}
