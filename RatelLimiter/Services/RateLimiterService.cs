using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
namespace RatelLimiter.Services
{

    public interface IRateLimiter
    {
        Task<bool> Check(string token);
        public long GetTimeRimining(string token);
    }

    public class RateLimiter: IRateLimiter
    {
        static readonly object objLock1 = new object();
        static readonly object objLock2 = new object();
        static readonly object objLockOnTimer = new object();

        int maxActionsInPeriod;
        int periodInMilSeconds;
        bool isBlocked;
        int actionsCount;
        Timer timer;
        Stopwatch stopwatch;

        public RateLimiter()
        {
            isBlocked = false;
            actionsCount = 0;
            timer = new Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(CheckTimer);
            timer.Start();
            stopwatch = new Stopwatch();
        }

        private void CheckTimer(object sender, ElapsedEventArgs e)
        {
            lock (objLockOnTimer)
            {
                actionsCount = 0;
            }

        }

        public async Task<bool> Check(string token)
        {

            lock (objLock1)
            {
                var limiter = Data.cache.ContainsKey(token) ? Data.cache[token] : null;
                if (limiter != null)
                {
                    if (limiter.CanPerformAction())
                    {
                        Data.cache[token].actionsCount++;
                        return true;
                    }
                    else
                    {
                        Data.cache[token].isBlocked = true;
                        if (!Data.cache[token].stopwatch.IsRunning)
                        {
                            Data.cache[token].stopwatch.Start();
                        }
                        else if ((Data.cache[token].stopwatch.ElapsedMilliseconds / 1000) >= 3600)
                        {
                            Data.cache[token].isBlocked = false;
                            Data.cache[token].stopwatch.Stop();
                            Data.cache[token].stopwatch.Reset();
                        }

                        return false;
                    }

                }
                else
                {
                    var newLimiter = new RateLimiter();
                    newLimiter.periodInMilSeconds = 60000;
                    newLimiter.maxActionsInPeriod = 10;
                    Data.cache.Add(token, newLimiter);
                    return true;
                }
            }
        }

        private bool CanPerformAction()
        {
            if ((actionsCount < maxActionsInPeriod) && !isBlocked)
                return true;
            else
                return false;
        }

        public long GetTimeRimining(string token)
        {

            lock(objLock2)
            {
                return (((60*60) - (Data.cache[token].stopwatch.ElapsedMilliseconds / 1000))/60);
            }
        }
    }    
}
