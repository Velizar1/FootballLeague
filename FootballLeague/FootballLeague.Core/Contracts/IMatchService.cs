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
        public Task<RepositoryResult> AddMatch<T>(MatchModel<T> matchModel);

        public Task<RepositoryResult> RemoveMatch<T>(T id);

        public Task<RepositoryResult> UdpateMatch<T>(MatchModel<T> matchModel);

        public List<MatchModel<T>> AllMatches<T>();

        public Task<MatchModel<T>> GetMatchById<T>(T id);
        public Task<MatchModel<T>> GetMatchByTeams<T>(T visitngTeamId, T hostingTeamId);
    }
}
