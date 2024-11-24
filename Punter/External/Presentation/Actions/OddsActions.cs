using Handler.Application.Publish;
using Handler.Domain;
using System.Text.Json;

namespace Punter.Presentation.Actions
{
    internal class OddsActions : IAsyncDisposable
    {
        private readonly IMessageQueueProvider _messageQueueProvider;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private bool _disposed = false;

        public OddsActions(IMessageQueueProvider messageQueueProvider)
        {
            _messageQueueProvider = messageQueueProvider;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task SubscribeAsync()
        {
            await _messageQueueProvider.SubscribeAsync(messageHandler, _cancellationTokenSource.Token);
        }

        private void messageHandler(string message)
        {
            var odds = JsonSerializer.Deserialize<IEnumerable<Odds>>(message);

            if (odds != null && odds.Any() == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("New odds inbound, updating..");
                Console.ForegroundColor = ConsoleColor.Blue;
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("Punter View");

                var counter = 1;
                foreach (var odd in odds)
                {
                    Console.WriteLine($"#{counter++} | {odd.HomeTeam} vs {odd.AwayTeam} | Home Win - {odd.HomeOdds} | Draw - {odd.DrawOdds} | Away Win - {odd.AwayOdds}");
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();

            _disposed = true;
        }
    }
}
