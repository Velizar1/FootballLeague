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

        public int TeamScore { get; set; }
        public List<MatchEditModel> HostedMatches { get; set; } 
        public List<MatchEditModel> VisitedMatches { get; set; } 
    }
}
