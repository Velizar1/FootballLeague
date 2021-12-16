using FootballLeague.DataAccess.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Contracts
{
    public interface IScoreService
    {
        public Task CalculateScore<T>(Match<T> match, T VisitingTeamId, T HostingTeamId, bool revertScore = false);
    }
}
