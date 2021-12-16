using System;
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

        public const string SaveFailed = "Save failed";

        public const string CreateFailed = "Couldnt save entity";
        public const string DeleteFailed = "Couldnt remove entity";
        public const string UpdateFailed = "Couldnt update entity";

        public const string Exist = "Entity already exists";
    }
}
