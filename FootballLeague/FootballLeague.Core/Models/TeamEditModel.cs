using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Models
{
    public class TeamEditModel
    {
        
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long? TeamScore { get; set; }

      
    }
}
