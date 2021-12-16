
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
        private readonly ITeamService teamService;
        public MatchService(IRepository _repo, ITeamService _teamService)
        {
            this.repo = _repo;
            this.teamService = _teamService;
        }
        public async Task<RepositoryResult> AddMatchAsync<T>(MatchModel<T> matchModel)
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
                });

                await repo.SavechangesAsync();

                await CalculateScore<T>(null, matchModel.VisitingTeamId, matchModel.HostingTeamId);

                result.IsSuccess = ResultConstants.Success;
                result.Message = ResultConstants.CreateSucceeded;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MatchModel<T>> AllMatches<T>()
        {
            return repo.AllReadOnly<Match<T>>()
                .Select(x => new MatchModel<T>
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

        public async Task<MatchModel<T>> GetMatchByTeamsAsync<T>(T visitngTeamId, T hostingTeamId)
        {
            return await repo.AllReadOnly<Match<T>>(x => x.VisitingTeamId.Equals(visitngTeamId ?? x.VisitingTeamId) &&
                                                   x.HostingTeamId.Equals(hostingTeamId ?? x.HostingTeamId)
                                                   )
                .Select(x => new MatchModel<T>()
                {
                    Id = x.Id,
                    HostingTeamId = x.HostingTeamId,
                    VisitingTeamId = x.VisitingTeamId,
                    VisitingTeamScore = x.VisitingTeamScore,
                    HostingTeamScore = x.HostingTeamScore,
                    IsPlayed = x.IsPlayed,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<MatchModel<T>> GetMatchByIdAsync<T>(T id)
        {
            return await repo.AllReadOnly<Match<T>>(x => x.Id.Equals(id))
                 .Select(x => new MatchModel<T>()
                 {
                     Id = x.Id,
                     HostingTeamId = x.HostingTeamId,
                     VisitingTeamId = x.VisitingTeamId,
                     VisitingTeamScore = x.VisitingTeamScore,
                     HostingTeamScore = x.HostingTeamScore,
                     IsPlayed = x.IsPlayed,
                 })
                 .FirstOrDefaultAsync();
        }

        public async Task<RepositoryResult> RemoveMatchAsync<T>(T id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);
            try
            {
                var data = await repo.All<Match<T>>(x => x.Id.Equals(id))
                   .FirstOrDefaultAsync();
                await CalculateScore<T>(data, data.HostingTeamId, data.VisitingTeamId, true);
                if (data != null)
                {
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

        public async Task<RepositoryResult> UdpateMatchAsync<T>(MatchModel<T> matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var match = await repo.All<Match<T>>(x => x.Id.Equals(matchModel.Id))
                   .FirstOrDefaultAsync();
                if (match != null)
                {
                    await CalculateScore<T>(match, matchModel.VisitingTeamId, matchModel.HostingTeamId, true);
                    match.VisitingTeamId = matchModel.VisitingTeamId;
                    match.VisitingTeamScore = matchModel.VisitingTeamScore;
                    match.HostingTeamId = matchModel.HostingTeamId;
                    match.HostingTeamScore = matchModel.HostingTeamScore;
                    result = repo.Update<Match<T>>(match);
                    await repo.SavechangesAsync();
                    await CalculateScore<T>(match, matchModel.VisitingTeamId, matchModel.HostingTeamId);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task CalculateScore<T>(Match<T> match, T VisitingTeamId, T HostingTeamId, bool revertScore = false)
        {
            if (match == null)
            {
                match = await repo.All<Match<T>>()
               .Include(x => x.HostingTeam)
               .Include(x => x.VisitingTeam)
               .Where(x => x.VisitingTeamId.Equals(VisitingTeamId) &&
                           x.HostingTeamId.Equals(HostingTeamId))
               .FirstOrDefaultAsync();
            }

            if (match != null)
            {
                var teamModel = new TeamModel<T>();
                try
                {
                    if (match.HostingTeamScore > match.VisitingTeamScore)
                    {
                        teamModel = await teamService.GetTeamByIdAsync<T>(match.HostingTeamId);
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -3 : 3);
                    }
                    else if (match.HostingTeamScore < match.VisitingTeamScore)
                    {
                        teamModel = await teamService.GetTeamByIdAsync<T>(match.VisitingTeamId);
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -3 : 3);
                    }
                    else
                    {
                        teamModel = await teamService.GetTeamByIdAsync<T>(match.VisitingTeamId);
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -1 : 1);
                        await teamService.UdpateTeamAsync<T>(teamModel);
                        teamModel = await teamService.GetTeamByIdAsync<T>(match.HostingTeamId);
                        teamModel.TeamScore = (teamModel.TeamScore ?? 0) + (revertScore ? -1 : 1);

                    }
                    await teamService.UdpateTeamAsync<T>(teamModel);
                    await repo.SavechangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

    }
}
