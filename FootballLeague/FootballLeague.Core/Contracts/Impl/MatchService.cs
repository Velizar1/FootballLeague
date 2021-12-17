
using FootballLeague.Core.Commons;
using FootballLeague.Core.Constants;
using FootballLeague.Core.Models;
using FootballLeague.Core.Repositories;
using FootballLeague.DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Contracts.Impl
{
    public class MatchService : IMatchService
    {
        private readonly IRepository repo;
        private readonly IScoreService scoreService;
        public MatchService(IRepository _repo, IScoreService _scoreService)
        {
            repo = _repo;
            scoreService = _scoreService;
        }

        public async Task<RepositoryResult> AddMatchAsync(MatchModel matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);

            var match = await repo.AllReadOnly<Match>()
                .Where(x => x.HostingTeamId.Equals(matchModel.HostingTeamId) &&
                          x.VisitingTeamId.Equals(matchModel.VisitingTeamId))
                .FirstOrDefaultAsync();

            if (match != null)
            {
                result.Message = ResultConstants.Exist;
                return result;
            }

            result = await repo.CreateAsync<Match>(new Match()
            {
                HostingTeamId = matchModel.HostingTeamId,
                VisitingTeamId = matchModel.VisitingTeamId,
                HostingTeamScore = matchModel.HostingTeamScore,
                VisitingTeamScore = matchModel.VisitingTeamScore,

            });

            await repo.SavechangesAsync();

            await scoreService.CalculatePoints(null, matchModel.VisitingTeamId, matchModel.HostingTeamId);

            result.IsSuccess = ResultConstants.Success;
            result.Message = ResultConstants.CreateSucceeded;
            return result;

        }

        public IEnumerable<MatchModel> AllMatches()
        {
            var matches = repo.AllReadOnly<Match>()
                .Select(x => new MatchModel
                {
                    Id = x.Id,
                    HostingTeamId = x.HostingTeamId,
                    VisitingTeamId = x.VisitingTeamId,
                    HostingTeamScore = x.HostingTeamScore,
                    VisitingTeamScore = x.VisitingTeamScore,
                })
                .ToList();

            if (matches.Count == 0)
            {
                throw new InvalidOperationException(ExceptionConstants.Message.EmptySequence);
            }
            return matches;
        }

        public async Task<MatchModel> GetMatchByIdAsync(Guid id)
        {
            var match = await repo.AllReadOnly<Match>(x => x.Id.Equals(id))
                 .Select(x => new MatchModel()
                 {
                     Id = x.Id,
                     HostingTeamId = x.HostingTeamId,
                     VisitingTeamId = x.VisitingTeamId,
                     VisitingTeamScore = x.VisitingTeamScore,
                     HostingTeamScore = x.HostingTeamScore,

                 })
                 .FirstOrDefaultAsync();

            if (match == null)
            {
                throw new ArgumentException(ExceptionConstants.Message.NotFoundById);
            }
            return match;
        }

        public async Task<RepositoryResult> RemoveMatchAsync(Guid id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);

            var match = await repo.All<Match>(x => x.Id.Equals(id))
                .Include(x => x.VisitingTeam)
                .Include(x => x.HostingTeam)
               .FirstOrDefaultAsync();

            if (match == null)
            {
                throw new ArgumentException(ExceptionConstants.Message.NotFoundById);
            }

            await scoreService.CalculatePoints(match, match.HostingTeamId, match.VisitingTeamId, true);
            result = repo.Delete(match);
            await repo.SavechangesAsync();
            return result;

        }

        public async Task<RepositoryResult> UdpateMatchAsync(MatchModel matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);

            var match = await repo.All<Match>(x => x.Id.Equals(matchModel.Id))
                .Include(x => x.VisitingTeam)
                .Include(x => x.HostingTeam)
                .FirstOrDefaultAsync();

            if (match == null)
            {
                throw new ArgumentException(ExceptionConstants.Message.BadArguments);
            }

            await scoreService.CalculatePoints(match, match.VisitingTeamId, match.HostingTeamId, true);
            match.VisitingTeamId = matchModel.VisitingTeamId;
            match.VisitingTeamScore = matchModel.VisitingTeamScore;
            match.HostingTeamId = matchModel.HostingTeamId;
            match.HostingTeamScore = matchModel.HostingTeamScore;
            result = repo.Update(match);
            await repo.SavechangesAsync();
            await scoreService.CalculatePoints(match, matchModel.VisitingTeamId, matchModel.HostingTeamId);
            return result;

        }
    }
}
