using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone
{
    public class CookieRenewService : IDisposable
    {
        private CancellationTokenSource? _cts;
        private Task? _renewTask;

        public void Start()
        {
            if (_renewTask != null && !_renewTask.IsCompleted)
                return;

            _cts = new CancellationTokenSource();
            _renewTask = RenewLoopAsync(_cts.Token);
        }

        public async Task StopAsync()
        {
            if (_cts == null)
                return;

            _cts.Cancel();

            try
            {
                if (_renewTask != null)
                    await _renewTask;
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task RenewLoopAsync(CancellationToken token)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(0.5));

            try
            {
                while (await timer.WaitForNextTickAsync(token))
                {
                    await RenewCookieAsync(token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task RenewCookieAsync(CancellationToken token)
        {
            await ApiTax.RenewCookie();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
 }
