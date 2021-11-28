using System.Collections.Generic;

namespace RatelLimiter.Services
{
    public static class Data
    {
        public static Dictionary<string, RateLimiter> cache = new Dictionary<string, RateLimiter>();
    }
}
