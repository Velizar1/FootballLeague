using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Models
{
    public class TeamModel<T>
    {
        
        public T Id { get; set; }

        public string Name { get; set; }

        public long? TeamScore { get; set; }

        public List<MatchModel<T>> HostedMatches { get; set; } = new List<MatchModel<T>>();
        public List<MatchModel<T>> VisitedMatches { get; set; } = new List<MatchModel<T>>();
    }
}
