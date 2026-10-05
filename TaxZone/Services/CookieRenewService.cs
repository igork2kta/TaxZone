
namespace TaxZone.Services
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
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(3));

            try
            {
                while (await timer.WaitForNextTickAsync(token))
                {
                    if(!await RenewCookieAsync(token)) return;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task<bool> RenewCookieAsync(CancellationToken token)
        {
            return await ApiTax.RenewCookie();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
 }
