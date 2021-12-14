using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.DataAccess.Data.Models
{
    public class Match<T>
    {
        [Key]
        public T Id { get; set; }
        public bool IsPlayed { get; set; }

        public T HostingTeamId;
        public T VisitingTeamId { get; set; }

        
        public int HostingTeamScore { get; set; }
        public int VisitingTeamScore { get; set; }

        //see if virtual needed

        [ForeignKey(nameof(HostingTeamId))]
        public Team<T> HostingTeam { get; set; }

        [ForeignKey(nameof(VisitingTeamId))]
        public Team<T> VisitingTeam { get; set; }
    }
}
