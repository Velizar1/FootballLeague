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


        public async Task<RepositoryResult> AddTeamAsync(TeamAddModel teamModel)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);
            try
            {
                var team = await repo.AllReadOnly<Team>()
                    .Where(x => x.Name.Equals(teamModel.Name))
                    .FirstOrDefaultAsync();
                if (team != null)
                {
                    result.Message = ResultConstants.Exist;
                    return result;
                }
                result = await repo.CreateAsync(new Team()
                {

                    Name = teamModel.Name ?? string.Empty,
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
      
        public List<TeamModel> AllTeams()
        {
            return repo.AllReadOnly<Team>()
                .Include(x => x.VisitedMatches)
                .Include(x => x.HostedMatches)
                .Select(x => new TeamModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TeamScore = x.TeamScore,
                    HostedMatches = x.HostedMatches.Select(y => new MatchEditModel()
                    {
                        Id = y.Id,
                        HostingTeamId = y.HostingTeamId,
                        HostingTeamScore = y.HostingTeamScore,
                        VisitingTeamScore = y.VisitingTeamScore,
                        VisitingTeamId = y.VisitingTeamId,
                       

                    }).ToList(),
                    VisitedMatches = x.VisitedMatches.Select(y => new MatchEditModel()
                    {
                        Id = y.Id,
                        HostingTeamId = y.HostingTeamId,
                        HostingTeamScore = y.HostingTeamScore,
                        VisitingTeamScore = y.VisitingTeamScore,
                        VisitingTeamId = y.VisitingTeamId,
                       

                    }).ToList()

                })
                .ToList();

        }

        public async Task<TeamModel> GetTeamByIdAsync(Guid Id)
        {
            var team = await repo.AllReadOnly<Team>(x => x.Id.Equals(Id))
                .Include(x => x.HostedMatches)
                .Include(x => x.VisitedMatches)
               .Select(x => new TeamModel()
               {
                   Id = x.Id,
                   Name = x.Name,
                   TeamScore = x.TeamScore,
                   HostedMatches = x.HostedMatches.Select(y => new MatchEditModel()
                   {
                       Id = y.Id,
                       HostingTeamId = y.HostingTeamId,
                       HostingTeamScore = y.HostingTeamScore,
                       VisitingTeamScore = y.VisitingTeamScore,
                       VisitingTeamId = y.VisitingTeamId,
                      

                   }).ToList(),
                   VisitedMatches = x.VisitedMatches.Select(y => new MatchEditModel()
                   {
                       Id = y.Id,
                       HostingTeamId = y.HostingTeamId,
                       HostingTeamScore = y.HostingTeamScore,
                       VisitingTeamScore = y.VisitingTeamScore,
                       VisitingTeamId = y.VisitingTeamId,
                      

                   }).ToList()
               })
               .FirstOrDefaultAsync();

            if (team == null)
            {
                throw new Exception(ResultConstants.NotFound);
            }
            return team;
        }

        public async Task<RepositoryResult> RemoveTeamAsync(Guid id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);
            try
            {

                var team = await repo.All<Team>(x => x.Id.Equals(id))
                   .FirstOrDefaultAsync();
                if (team != null)
                {
                    var matches = repo.All<Match>()
                    .Where(x => x.VisitingTeamId.Equals(id) || x.HostingTeamId.Equals(id))
                    .ToList();

                    foreach (var match in matches)
                    {
                        await matchService.RemoveMatchAsync(match.Id);
                    }

                    result = repo.Delete<Team>(team);
                    await repo.SavechangesAsync();

                }

                return result;
            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task<RepositoryResult> UdpateTeamAsync(TeamEditModel teamModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var team = await repo.All<Team>(x => x.Id.Equals(teamModel.Id))
                   .FirstOrDefaultAsync();
                if (team != null)
                {
                    team.Name = teamModel.Name ?? String.Empty;
                    team.TeamScore = teamModel.TeamScore;

                    result = repo.Update<Team>(team);
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
