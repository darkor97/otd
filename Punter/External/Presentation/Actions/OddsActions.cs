using Handler.Application.Publish;
using Handler.Domain;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Text.Json;

namespace Punter.Presentation.Actions
{
    internal class OddsActions : IAsyncDisposable
    {
        private readonly IMessageQueueProvider _messageQueueProvider;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private const int MaxRetry = 5;
        private bool _disposed = false;

        public OddsActions(IMessageQueueProvider messageQueueProvider)
        {
            _messageQueueProvider = messageQueueProvider;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task SubscribeAsync()
        {
            var policy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    MaxRetry,
                    (attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        if (retryCount == MaxRetry)
                        {
                            Console.WriteLine("RabbitMQ count not be reached. Stopping application..");
                        }
                        Console.WriteLine("RabbitMq not ready, retrying");
                    });

            await policy.ExecuteAsync(() => _messageQueueProvider.SubscribeAsync(messageHandler, _cancellationTokenSource.Token));
        }

        private Task messageHandler(string message)
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

            Console.ForegroundColor = ConsoleColor.White;
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();

            Console.ForegroundColor = ConsoleColor.White;

            _disposed = true;
        }
    }
}
