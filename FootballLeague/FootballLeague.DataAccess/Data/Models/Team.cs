using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.DataAccess.Data.Models
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public long? TeamScore { get; set; }

        public ICollection<Match> HostedMatches { get; set; }
        public ICollection<Match> VisitedMatches { get; set; }

        public Team()
        {
            HostedMatches = new List<Match>();
            VisitedMatches = new List<Match>();
        }
    }
}
