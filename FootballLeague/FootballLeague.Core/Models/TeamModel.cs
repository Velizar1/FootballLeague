using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Models
{
   public  class TeamModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TeamPoints { get; set; }
        public IEnumerable<MatchModel> HostedMatches { get; set; } 
        public IEnumerable<MatchModel> VisitedMatches { get; set; } 
    }
}
