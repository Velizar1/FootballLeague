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

        public List<Match<T>> HostedMatches { get; set; }
        public List<Match<T>> VisitedMatches { get; set; }
    }
}
