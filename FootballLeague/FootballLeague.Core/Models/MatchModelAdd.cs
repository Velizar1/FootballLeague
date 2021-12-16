﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Models
{
    public class MatchModelAdd<T>
    {
       
        public T HostingTeamId { get; set; }
        public T VisitingTeamId { get; set; }
        public int HostingTeamScore { get; set; }
        public int VisitingTeamScore { get; set; }
        public bool IsPlayed { get; set; }
    }
}
