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
    public class TeamService : ITeamService
    {
        private readonly IRepository repo;
        private readonly IMatchService matchService;


        public TeamService(IRepository _repo, IMatchService _matchService)
        {
            this.repo = _repo;
            this.matchService = _matchService;
        }


        public async Task<RepositoryResult> AddTeamAsync<T>(TeamModelAdd<T> teamModel)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);
            try
            {
                var team = await repo.AllReadOnly<Team<T>>()
                    .Where(x => x.Name.Equals(teamModel.Name))
                    .FirstOrDefaultAsync();
                if (team != null)
                {
                    result.Message = ResultConstants.Exist;
                    return result;
                }
                result = await repo.CreateAsync<Team<T>>(new Team<T>()
                {

                    Name = teamModel.Name ?? String.Empty,
                    TeamScore = teamModel.TeamScore,
                });

                await repo.SavechangesAsync();

                result.IsSuccess = ResultConstants.Success;
                result.Message = ResultConstants.CreateSucceeded;

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<RepositoryResult> AddHostedMatchAsync<T>(T TeamId, MatchModelEdit<T> matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var team = await repo.All<Team<T>>(x => x.Id.Equals(TeamId) && x.Id.Equals(matchModel.HostingTeamId))
                    .Include(x => x.HostedMatches)
                   .FirstOrDefaultAsync();
                if (team != null)
                {
                    var match = await repo.AllReadOnly<Match<T>>()
                                        .Where(x => x.Id.Equals(matchModel.Id) &&
                                                  x.HostingTeamId.Equals(team.Id))
                                        .FirstOrDefaultAsync();
                    if (match != null)
                    {
                        team.HostedMatches.Add(match);
                        result = repo.Update<Team<T>>(team);
                        await repo.SavechangesAsync();
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RepositoryResult> AddVisitedMatchAsync<T>(T TeamId, MatchModelEdit<T> matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var team = await repo.All<Team<T>>(x => x.Id.Equals(TeamId) && x.Id.Equals(matchModel.VisitingTeamId))
                    .Include(x => x.VisitedMatches)
                   .FirstOrDefaultAsync();
                if (team != null)
                {
                    var match = await repo.AllReadOnly<Match<T>>()
                                        .Where(x => x.Id.Equals(matchModel.Id) &&
                                                  x.VisitingTeamId.Equals(team.Id))
                                        .FirstOrDefaultAsync();
                    if (match != null)
                    {
                        team.VisitedMatches.Add(match);
                        result = repo.Update<Team<T>>(team);
                        await repo.SavechangesAsync();
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw;

            }
        }

        public List<TeamModel<T>> AllTeams<T>()
        {
            return repo.AllReadOnly<Team<T>>()
                .Include(x => x.VisitedMatches)
                .Include(x => x.HostedMatches)
                .Select(x => new TeamModel<T>
                {
                    Id = x.Id,
                    Name = x.Name,
                    TeamScore = x.TeamScore,
                    HostedMatches = x.HostedMatches.Select(y => new MatchModelEdit<T>()
                    {
                        Id = y.Id,
                        HostingTeamId = y.HostingTeamId,
                        HostingTeamScore = y.HostingTeamScore,
                        VisitingTeamScore = y.VisitingTeamScore,
                        VisitingTeamId = y.VisitingTeamId,
                        IsPlayed = y.IsPlayed,

                    }).ToList(),
                    VisitedMatches = x.VisitedMatches.Select(y => new MatchModelEdit<T>()
                    {
                        Id = y.Id,
                        HostingTeamId = y.HostingTeamId,
                        HostingTeamScore = y.HostingTeamScore,
                        VisitingTeamScore = y.VisitingTeamScore,
                        VisitingTeamId = y.VisitingTeamId,
                        IsPlayed = y.IsPlayed,

                    }).ToList()

                })
                .ToList();

        }

        public async Task<TeamModel<T>> GetTeamByIdAsync<T>(T Id)
        {
            var team = await repo.AllReadOnly<Team<T>>(x => x.Id.Equals(Id))
                .Include(x => x.HostedMatches)
                .Include(x => x.VisitedMatches)
               .Select(x => new TeamModel<T>()
               {
                   Id = x.Id,
                   Name = x.Name,
                   TeamScore = x.TeamScore,
                   HostedMatches = x.HostedMatches.Select(y => new MatchModelEdit<T>()
                   {
                       Id = y.Id,
                       HostingTeamId = y.HostingTeamId,
                       HostingTeamScore = y.HostingTeamScore,
                       VisitingTeamScore = y.VisitingTeamScore,
                       VisitingTeamId = y.VisitingTeamId,
                       IsPlayed = y.IsPlayed,

                   }).ToList(),
                   VisitedMatches = x.VisitedMatches.Select(y => new MatchModelEdit<T>()
                   {
                       Id = y.Id,
                       HostingTeamId = y.HostingTeamId,
                       HostingTeamScore = y.HostingTeamScore,
                       VisitingTeamScore = y.VisitingTeamScore,
                       VisitingTeamId = y.VisitingTeamId,
                       IsPlayed = y.IsPlayed,

                   }).ToList()
               })
               .FirstOrDefaultAsync();

            if (team == null)
            {
                throw new Exception(ResultConstants.NotFound);
            }
            return team;
        }

        public async Task<RepositoryResult> RemoveTeamAsync<T>(T id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);
            try
            {

                var team = await repo.All<Team<T>>(x => x.Id.Equals(id))
                   .FirstOrDefaultAsync();
                if (team != null)
                {
                    var matches = repo.All<Match<T>>()
                    .Where(x => x.VisitingTeamId.Equals(id) || x.HostingTeamId.Equals(id))
                    .ToList();

                    foreach (var match in matches)
                    {
                        await matchService.RemoveMatchAsync(match.Id);
                    }

                    result = repo.Delete<Team<T>>(team);
                    await repo.SavechangesAsync();

                }

                return result;
            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task<RepositoryResult> UdpateTeamAsync<T>(TeamModelEdit<T> teamModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var team = await repo.All<Team<T>>(x => x.Id.Equals(teamModel.Id))
                   .FirstOrDefaultAsync();
                if (team != null)
                {
                    team.Name = teamModel.Name ?? String.Empty;
                    team.TeamScore = teamModel.TeamScore;

                    result = repo.Update<Team<T>>(team);
                    await repo.SavechangesAsync();
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
