
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
            this.repo = _repo;
            this.scoreService = _scoreService;
        }
        public async Task<RepositoryResult> AddMatchAsync(MatchAddModel matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);
            try
            {
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
                   
                }) ;

                await repo.SavechangesAsync();

                await scoreService.CalculateScore(null, matchModel.VisitingTeamId, matchModel.HostingTeamId);

                result.IsSuccess = ResultConstants.Success;
                result.Message = ResultConstants.CreateSucceeded;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MatchEditModel> AllMatches()
        {
            return repo.AllReadOnly<Match>()
                .Select(x => new MatchEditModel
                {
                    Id = x.Id,
                    HostingTeamId = x.HostingTeamId,
                    VisitingTeamId = x.VisitingTeamId,
                    HostingTeamScore = x.HostingTeamScore,
                    VisitingTeamScore = x.VisitingTeamScore,
                   

                })
                .ToList();
        }

        public async Task<MatchEditModel> GetMatchByIdAsync(Guid id)
        {
            var data= await repo.AllReadOnly<Match>(x => x.Id.Equals(id))
                 .Select(x => new MatchEditModel()
                 {
                     Id = x.Id,
                     HostingTeamId = x.HostingTeamId,
                     VisitingTeamId = x.VisitingTeamId,
                     VisitingTeamScore = x.VisitingTeamScore,
                     HostingTeamScore = x.HostingTeamScore,
                   
                 })
                 .FirstOrDefaultAsync();
            if (data == null)
            {
                throw new Exception(ResultConstants.NotFound);
            }
            return data;
        }

        public async Task<RepositoryResult> RemoveMatchAsync(Guid id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);
            try
            {
                var data = await repo.All<Match>(x => x.Id.Equals(id))
                    .Include(x=>x.VisitingTeam)
                    .Include(x=>x.HostingTeam)
                   .FirstOrDefaultAsync();
               
                if (data != null)
                {
                    await scoreService.CalculateScore(data, data.HostingTeamId, data.VisitingTeamId, true);
                    result = repo.Delete<Match>(data);
                    await repo.SavechangesAsync();

                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RepositoryResult> UdpateMatchAsync(MatchEditModel matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var match = await repo.All<Match>(x => x.Id.Equals(matchModel.Id))
                    .Include(x => x.VisitingTeam)
                    .Include(x => x.HostingTeam)
                   .FirstOrDefaultAsync();
                if (match != null)
                {
                    await scoreService.CalculateScore(match, matchModel.VisitingTeamId, matchModel.HostingTeamId, true);
                    match.VisitingTeamId = matchModel.VisitingTeamId;
                    match.VisitingTeamScore = matchModel.VisitingTeamScore;
                    match.HostingTeamId = matchModel.HostingTeamId;
                    match.HostingTeamScore = matchModel.HostingTeamScore;
                    result = repo.Update<Match>(match);
                    await repo.SavechangesAsync();
                    await scoreService.CalculateScore(match, matchModel.VisitingTeamId, matchModel.HostingTeamId);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

       

    }
}
