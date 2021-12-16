using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Models
{
   public  class TeamModel<T>
    {
        public T Id { get; set; }

        public string Name { get; set; }

        public long? TeamScore { get; set; }
        public List<MatchModelEdit<T>> HostedMatches { get; set; } = new List<MatchModelEdit<T>>();
        public List<MatchModelEdit<T>> VisitedMatches { get; set; } = new List<MatchModelEdit<T>>();
    }
}
