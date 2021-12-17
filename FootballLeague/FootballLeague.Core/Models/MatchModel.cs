using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Models
{
    public class MatchModel
    {
        public Guid Id { get; set; }
        public Guid HostingTeamId { get; set; }
        public Guid VisitingTeamId { get; set; }
        public int HostingTeamScore { get; set; }
        public int VisitingTeamScore { get; set; }
       
    }
}
