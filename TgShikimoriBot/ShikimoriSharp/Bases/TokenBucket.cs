using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ShikimoriSharp.Bases
{
    public class TokenBucket
    {
        private readonly SemaphoreSlim _sem;

        private readonly Timer _timer;

        public TokenBucket(string name, int maxTokens, double refreshTime)
        {
           
        }

        public string Name { get; }
        public int MaxTokens { get; }
        public double RefreshTime { get; }

        private void Refresh(object sender, ElapsedEventArgs args)
        {
            try
            {
                _sem.Release(MaxTokens);
            }
            catch(Exception) { }
        }

        public async Task TokenRequest()
        {
            
        }
    }
}