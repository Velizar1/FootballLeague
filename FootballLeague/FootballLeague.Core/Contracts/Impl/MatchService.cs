
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
        private IRepository repo;
        public MatchService(IRepository repository)
        {
            this.repo = repository;
        }
        public async Task<RepositoryResult> AddMatch<T>(MatchModel<T> matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.CreateFailed);
            try
            {
                result = await repo.CreateAsync<Match<T>>(new Match<T>()
                {
                    HostingTeamId = matchModel.HostingTeamId,
                    VisitingTeamId = matchModel.VisitingTeamId,
                    HostingTeamScore = matchModel.HostingTeamScore,
                    VisitingTeamScore = matchModel.VisitingTeamScore,
                    IsPlayed = matchModel.IsPlayed,
                });
                await repo.SavechangesAsync();
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
            return repo.AllReadOnly<Match<T>>().
                Select(x => new MatchModel<T>
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

        public async Task<MatchModel<T>> GetMatchByTeams<T>(T visitngTeamId, T hostingTeamId)
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

        public async Task<MatchModel<T>> GetMatchById<T>(T id)
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

        public async Task<RepositoryResult> RemoveMatch<T>(T id)
        {
            var result = new RepositoryResult(false, ResultConstants.DeleteFailed);
            try
            {
                var data = await repo.All<Match<T>>(x => x.Id.Equals(id))
                   .FirstOrDefaultAsync();
                if (data != null)
                {
                    result = repo.Delete<Match<T>>(data);
                    await repo.SavechangesAsync();
                    result.IsSuccess = ResultConstants.Success;
                    result.Message = ResultConstants.DeleteSucceeded;
                }

                return result;
            }
            catch (Exception)
            {
                return result;
                throw;
            }
        }

        public async Task<RepositoryResult> UdpateMatch<T>(MatchModel<T> matchModel)
        {
            var result = new RepositoryResult(false, ResultConstants.UpdateFailed);
            try
            {
                var data = await repo.All<Match<T>>(x => x.Id.Equals(matchModel.Id))
                   .FirstOrDefaultAsync();
                if (data != null)
                {
                    result = repo.Update<Match<T>>(data);
                    await repo.SavechangesAsync();
                    result.IsSuccess = ResultConstants.Success;
                    result.Message = ResultConstants.UpdateSucceeded;
                }
                return result;
            }
            catch (Exception)
            {
                return result;
                throw;
            }
        }
    }
}
