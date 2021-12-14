﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Constants
{
    public class ResultConstants
    {
        public const bool Success = true;
        public const bool Fail = false;

        public const string CreateSucceeded = "Successfully added entity";
        public const string DeleteSucceeded = "Successfully removed entity";
        public const string UpdateSucceeded = "Successfully updated entity";

        public const string CreateFail = "Couldnt save entity";
        public const string DeleteFail = "Couldnt remove entity";
        public const string UpdateFail = "Couldnt update entity";
    }
}