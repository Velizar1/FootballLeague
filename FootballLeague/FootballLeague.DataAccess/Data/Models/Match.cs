using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.DataAccess.Data.Models
{
    public class Match
    {
        [Key]
        public Guid Id { get; set; }
      
        public Guid HostingTeamId;
        public Guid VisitingTeamId { get; set; }

        public int HostingTeamScore { get; set; }
        public int VisitingTeamScore { get; set; }


        [ForeignKey(nameof(HostingTeamId))]
        public Team HostingTeam { get; set; }

        [ForeignKey(nameof(VisitingTeamId))]
        public Team VisitingTeam { get; set; }
    }
}
