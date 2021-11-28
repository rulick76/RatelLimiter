using RatelLimiter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatelLimiter.Services
{
    public static class Data
    {
        public static Dictionary<string, RateLimiter> cache = new Dictionary<string, RateLimiter>();
    }
}
