
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

                await CalculateNewScore<T>(matchModel.VisitingTeamId, matchModel.HostingTeamId);

                result.IsSuccess = ResultConstants.Success;
                result.Message = ResultConstants.CreateSucceeded;
                return result;
            }
            catch (Exception)
            {
                return result;
                throw new Exception(result.Message);
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
                await UpdateScore<T>(data, data.HostingTeamScore, data.VisitingTeamScore);
                if (data != null)
                {
                    result = repo.Delete<Match<T>>(data);
                    await repo.SavechangesAsync();

                }

                return result;
            }
            catch (Exception)
            {
                return new RepositoryResult(false, ResultConstants.DeleteFailed);
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
                    await UpdateScore<T>(match, matchModel.VisitingTeamScore, matchModel.HostingTeamScore);
                    match.VisitingTeamId = matchModel.VisitingTeamId;
                    match.VisitingTeamScore = matchModel.VisitingTeamScore;
                    match.HostingTeamId = matchModel.HostingTeamId;
                    match.HostingTeamScore = matchModel.HostingTeamScore;
                    result = repo.Update<Match<T>>(match);
                    await repo.SavechangesAsync();
                }
                return result;
            }
            catch (Exception)
            {
                return new RepositoryResult(false, ResultConstants.UpdateFailed);
                throw;
            }
        }
        //TODO
        private async Task CalculateNewScore<T>(Match<T> match, T VisitingTeamId, T HostingTeamId,bool isRemoved=false)
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

                if (match.HostingTeamScore > match.VisitingTeamScore)
                {
                    teamModel = await teamService.GetTeamByIdAsync<T>(HostingTeamId);
                    teamModel.TeamScore = (teamModel.TeamScore ?? 0) + 3;
                }
                else if (match.HostingTeamScore < match.VisitingTeamScore)
                {
                    teamModel = await teamService.GetTeamByIdAsync<T>(VisitingTeamId);
                    teamModel.TeamScore = (teamModel.TeamScore ?? 0) + 3;
                }
                else
                {
                    teamModel = await teamService.GetTeamByIdAsync<T>(VisitingTeamId);
                    teamModel.TeamScore = (teamModel.TeamScore ?? 0) + 1;
                    await teamService.UdpateTeamAsync<T>(teamModel);
                    teamModel = await teamService.GetTeamByIdAsync<T>(HostingTeamId);
                    teamModel.TeamScore = (teamModel.TeamScore ?? 0) + 1;

                }
                await teamService.UdpateTeamAsync<T>(teamModel);
            }

        }
        //TODO
        private async Task Recalculate<T>(Match<T> match, T VisitingTeamId , T HostedTeamId )
        {
            if (match != null)
            {
                var team = new TeamModel<T>();
                if (VisitingTeamId == null)
                {
                    team = await teamService.GetTeamByIdAsync<T>(VisitingTeamId);
                    if (match.VisitingTeamScore > match.HostingTeamScore)
                    {

                    }
                }
                if (HostedTeamId == null)
                {
                    team = await teamService.GetTeamByIdAsync<T>(HostedTeamId);
                }

                
            }
        }
        private async Task UpdateScore<T>(Match<T> match, int newScoreVisitingTeam, int newScoreHostedTeam)
        {


            if (match != null)
            {
                var newDifference = newScoreVisitingTeam - newScoreHostedTeam;
                var oldFifference = match.VisitingTeamScore - match.HostingTeamScore;

                var teamModelHosted = await teamService.GetTeamByIdAsync<T>(match.HostingTeamId);
                var teamModelVisited = await teamService.GetTeamByIdAsync<T>(match.VisitingTeamId);
                if (newDifference < 0 && oldFifference > 0)
                {
                    teamModelHosted.TeamScore = teamModelHosted.TeamScore + 3;
                    teamModelVisited.TeamScore = teamModelVisited.TeamScore - 3;
                }
                else if (newDifference > 0 && oldFifference < 0)
                {

                    teamModelHosted.TeamScore = teamModelHosted.TeamScore - 3;
                    teamModelVisited.TeamScore = teamModelVisited.TeamScore + 3;

                }
                else if (newDifference == 0 && oldFifference > 0)
                {

                    teamModelHosted.TeamScore = teamModelHosted.TeamScore + 1;
                    teamModelVisited.TeamScore = teamModelVisited.TeamScore - 2;

                }
                else if (newDifference == 0 && oldFifference < 0)
                {

                    teamModelHosted.TeamScore = teamModelHosted.TeamScore - 2;
                    teamModelVisited.TeamScore = teamModelVisited.TeamScore + 1;

                }
                else if (newDifference > 0 && oldFifference == 0)
                {

                    teamModelHosted.TeamScore = teamModelHosted.TeamScore - 1;
                    teamModelVisited.TeamScore = teamModelVisited.TeamScore + 2;

                }
                else if (newDifference < 0 && oldFifference == 0)
                {

                    teamModelHosted.TeamScore = teamModelHosted.TeamScore + 2;
                    teamModelVisited.TeamScore = teamModelVisited.TeamScore - 1;

                }
                await teamService.UdpateTeamAsync<T>(teamModelHosted);
                await teamService.UdpateTeamAsync<T>(teamModelVisited);
                await repo.SavechangesAsync();
            }

        }
    }
}
