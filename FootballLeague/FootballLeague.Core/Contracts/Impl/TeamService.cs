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
            repo = _repo;
            matchService = _matchService;
        }

        public async Task<RepositoryResult> AddTeamAsync(TeamModel teamModel)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);

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
                TeamPoints = teamModel.TeamPoints,
            });

            await repo.SavechangesAsync();
            result.IsSuccess = ResultConstants.Success;
            result.Message = ResultConstants.CreateSucceeded;

            return result;
        }
        
        public IEnumerable<TeamModel> AllTeams()
        {
            var teams = repo.AllReadOnly<Team>()
                .Include(x => x.VisitedMatches)
                .Include(x => x.HostedMatches)
                .Select(x => new TeamModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TeamPoints = x.TeamPoints,
                })
                .ToList();

            if (teams.Count == 0)
            {
                throw new InvalidOperationException(ExceptionConstants.Message.EmptySequence);
            }

            return teams;
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
                   TeamPoints = x.TeamPoints,
               })
               .FirstOrDefaultAsync();

            if (team == null)
            {
                throw new ArgumentException(ExceptionConstants.Message.NotFoundById);
            }
            return team;
        }

        public async Task<RepositoryResult> RemoveTeamAsync(Guid id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);

            var team = await repo.All<Team>(x => x.Id.Equals(id))
               .FirstOrDefaultAsync();

            if (team == null)
            {
                throw new ArgumentException(ExceptionConstants.Message.NotFoundById);
            }

            var matches = repo.All<Match>()
               .Where(x => x.VisitingTeamId.Equals(id) || x.HostingTeamId.Equals(id))
               .ToList();

            foreach (var match in matches)
            {
                await matchService.RemoveMatchAsync(match.Id);
            }

            result = repo.Delete(team);
            await repo.SavechangesAsync();
            return result;

        }

        public async Task<RepositoryResult> UdpateTeamAsync(TeamModel teamModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);

            var team = await repo.All<Team>(x => x.Id.Equals(teamModel.Id))
               .FirstOrDefaultAsync();

            if (team == null)
            {
                throw new ArgumentException(ExceptionConstants.Message.BadArguments); 
            }

            team.Name = teamModel.Name ?? string.Empty;
            team.TeamPoints = teamModel.TeamPoints;
            result = repo.Update(team);
            await repo.SavechangesAsync();
            return result;

        }
    }
}
