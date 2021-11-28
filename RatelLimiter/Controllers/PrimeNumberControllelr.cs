using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RatelLimiter.Helper;
using RatelLimiter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RatelLimiter.Models.Entities;

namespace RatelLimiter.Controllers
{
    [ApiController]
    [Route("RateLimit")]
    public class PrimeNumberControllelr : ControllerBase
    {

        private readonly ILogger<PrimeNumberControllelr> _logger;
        private IRateLimiter _rateLimiter;

        public PrimeNumberControllelr(ILogger<PrimeNumberControllelr> logger, IRateLimiter rateLimiter)
        {
            _logger = logger;
            _rateLimiter = rateLimiter;
        }

        [Route("IsPrimeNumber")]
        [HttpGet]
        public async Task<ActionResult<IsPrimeNumberResponseBase>> Get(string number, string token)
        {
            if (await _rateLimiter.Check(token))
            {
                int iNumber = Convert.ToInt32(number);
                IsPrimeNumberAllowedResponse response = new IsPrimeNumberAllowedResponse();
                response.HasErrors = false;
                response.Number = iNumber;
                response.IsPrime = CalculatePrime.isPrime(iNumber);
                return Ok(response);
            }
            else
            {
                IsPrimeNumberBlockedResponse response = new IsPrimeNumberBlockedResponse();
                response.HasErrors = true;
                response.ErrorMessage = "You have exceeded your call limits.Remaining time until restriction removed is " + _rateLimiter.GetTimeRimining(token) + " minutes";
                return Ok(response);
            }
        }
    }
}
