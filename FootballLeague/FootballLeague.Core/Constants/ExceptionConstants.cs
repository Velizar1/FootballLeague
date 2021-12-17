using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Constants
{
    public static class ExceptionConstants
    {
        public class Message
        {
            public const string EmptySequence = "Sequence contains no elements ";
            public const string BadArguments = "Bad arguments passed ";
            public const string NotFoundById = "Couldnt find entity by given Id ";
            public const string NotFound = "Couldnt find entity ";
            public const string DeleteFailed = "Couldnt remove entity ";

        }
    }
}
