using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorMovies.Client.Auth
{
    public class TokenRenewer : IDisposable
    {
        private Timer _timer;
        private readonly ILoginService _loginService;

        public TokenRenewer(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public void Initiate()
        {
            _timer = new Timer();
            _timer.Interval = 1000 * 60 * 4; // 4 minutes
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("timer elapsed");
            _loginService.TryRenewToken();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}