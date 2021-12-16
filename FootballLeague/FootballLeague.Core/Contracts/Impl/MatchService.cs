
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
        public async Task<RepositoryResult> AddMatchAsync<T>(MatchModelAdd<T> matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);
            try
            {
                var match = await repo.AllReadOnly<Match<T>>()
                    .Where(x => x.HostingTeamId.Equals(matchModel.HostingTeamId) &&
                              x.VisitingTeamId.Equals(matchModel.VisitingTeamId))
                    .FirstOrDefaultAsync();
                if (match != null)
                {
                    result.Message = ResultConstants.Exist;
                    return result;
                }
                result = await repo.CreateAsync<Match<T>>(new Match<T>()
                {
                    HostingTeamId = matchModel.HostingTeamId,
                    VisitingTeamId = matchModel.VisitingTeamId,
                    HostingTeamScore = matchModel.HostingTeamScore,
                    VisitingTeamScore = matchModel.VisitingTeamScore,
                    IsPlayed = matchModel.IsPlayed,
                }) ;

                await repo.SavechangesAsync();

                await scoreService.CalculateScore<T>(null, matchModel.VisitingTeamId, matchModel.HostingTeamId);

                result.IsSuccess = ResultConstants.Success;
                result.Message = ResultConstants.CreateSucceeded;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MatchModelEdit<T>> AllMatches<T>()
        {
            return repo.AllReadOnly<Match<T>>()
                .Select(x => new MatchModelEdit<T>
                {
                    Id = x.Id,
                    HostingTeamId = x.HostingTeamId,
                    VisitingTeamId = x.VisitingTeamId,
                    HostingTeamScore = x.HostingTeamScore,
                    VisitingTeamScore = x.VisitingTeamScore,
                    IsPlayed = x.IsPlayed

                })
                .ToList();
        }

        public async Task<MatchModelEdit<T>> GetMatchByTeamsAsync<T>(T visitngTeamId, T hostingTeamId)
        {
            var data= await repo.AllReadOnly<Match<T>>(x => x.VisitingTeamId.Equals(visitngTeamId ?? x.VisitingTeamId) &&
                                                   x.HostingTeamId.Equals(hostingTeamId ?? x.HostingTeamId)
                                                   )
                .Select(x => new MatchModelEdit<T>()
                {
                    Id = x.Id,
                    HostingTeamId = x.HostingTeamId,
                    VisitingTeamId = x.VisitingTeamId,
                    VisitingTeamScore = x.VisitingTeamScore,
                    HostingTeamScore = x.HostingTeamScore,
                    IsPlayed = x.IsPlayed,
                })
                .FirstOrDefaultAsync();
            if (data == null)
            {
                throw new Exception(ResultConstants.NotFound);
            }
            return data;
        }

        public async Task<MatchModelEdit<T>> GetMatchByIdAsync<T>(T id)
        {
            var data= await repo.AllReadOnly<Match<T>>(x => x.Id.Equals(id))
                 .Select(x => new MatchModelEdit<T>()
                 {
                     Id = x.Id,
                     HostingTeamId = x.HostingTeamId,
                     VisitingTeamId = x.VisitingTeamId,
                     VisitingTeamScore = x.VisitingTeamScore,
                     HostingTeamScore = x.HostingTeamScore,
                     IsPlayed = x.IsPlayed,
                 })
                 .FirstOrDefaultAsync();
            if (data == null)
            {
                throw new Exception(ResultConstants.NotFound);
            }
            return data;
        }

        public async Task<RepositoryResult> RemoveMatchAsync<T>(T id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);
            try
            {
                var data = await repo.All<Match<T>>(x => x.Id.Equals(id))
                    .Include(x=>x.VisitingTeam)
                    .Include(x=>x.HostingTeam)
                   .FirstOrDefaultAsync();
               
                if (data != null)
                {
                    await scoreService.CalculateScore<T>(data, data.HostingTeamId, data.VisitingTeamId, true);
                    result = repo.Delete<Match<T>>(data);
                    await repo.SavechangesAsync();

                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RepositoryResult> UdpateMatchAsync<T>(MatchModelEdit<T> matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var match = await repo.All<Match<T>>(x => x.Id.Equals(matchModel.Id))
                    .Include(x => x.VisitingTeam)
                    .Include(x => x.HostingTeam)
                   .FirstOrDefaultAsync();
                if (match != null)
                {
                    await scoreService.CalculateScore<T>(match, matchModel.VisitingTeamId, matchModel.HostingTeamId, true);
                    match.VisitingTeamId = matchModel.VisitingTeamId;
                    match.VisitingTeamScore = matchModel.VisitingTeamScore;
                    match.HostingTeamId = matchModel.HostingTeamId;
                    match.HostingTeamScore = matchModel.HostingTeamScore;
                    result = repo.Update<Match<T>>(match);
                    await repo.SavechangesAsync();
                    await scoreService.CalculateScore<T>(match, matchModel.VisitingTeamId, matchModel.HostingTeamId);
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
