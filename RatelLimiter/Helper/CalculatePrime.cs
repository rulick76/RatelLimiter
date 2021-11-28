using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatelLimiter.Helper
{
    public static class CalculatePrime
    {
        public static bool isPrime(int n)
        {
            if (n == 2)
            {
                return true;
            }
            if (n < 2 || n % 2 == 0)
            {
                return false;
            }
            for (int i = 3; i <= Math.Sqrt(n); i += 2)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

