using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatelLimiter.Models
{
    public class Entities
    {
        public class IsPrimeNumberResponseBase
        {
            public bool HasErrors { get; set; }
        }

        public class IsPrimeNumberAllowedResponse:IsPrimeNumberResponseBase
        {
            public int Number { get; set; }
            public bool IsPrime { get; set; }
        }

        public class IsPrimeNumberBlockedResponse : IsPrimeNumberResponseBase
        {
            public string ErrorMessage { get; set; }
        }
    }
}
