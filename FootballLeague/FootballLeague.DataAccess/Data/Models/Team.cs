using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.DataAccess.Data.Models
{
    public class Team<T> 
    {
        [Key]
        public T Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public long? TeamScore { get; set; }

        public ICollection<Match<T>> HostedMatches { get; set; }
        public ICollection<Match<T>> VisitedMatches { get; set; }

        public Team()
        {
            HostedMatches = new List<Match<T>>();
            VisitedMatches = new List<Match<T>>();
        }
    }
}
