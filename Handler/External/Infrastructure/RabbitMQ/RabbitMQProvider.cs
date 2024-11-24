using Handler.Application.Publish;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Handler.Infrastructure.Publish
{
    internal class RabbitMQProvider : IMessageQueueProvider
    {
        private const string QueueName = "otd";
        private const string Exchange = "odds";

        private readonly IConnectionFactory _factory;
        private readonly ILogger<RabbitMQProvider> _logger;

        public RabbitMQProvider(IConnectionFactory factory, ILogger<RabbitMQProvider> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task PublishAsync(string message, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.LogInformation("Empty argument: {message}, nothing published to the queue", message);
                return;
            }

            try
            {
                await using var connection = await _factory.CreateConnectionAsync(cancellationToken);
                await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

                await channel.QueueDeclareAsync(QueueName, true, false, false, cancellationToken: cancellationToken);
                await channel.ExchangeDeclareAsync(Exchange, ExchangeType.Fanout, cancellationToken: cancellationToken);

                var messageBody = Encoding.UTF8.GetBytes(message);
                await channel.BasicPublishAsync(Exchange, string.Empty, messageBody, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message to queue");
                throw;
            }
        }

        public async Task SubscribeAsync(Action<string> messageReceivedCallback, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var connection = await _factory.CreateConnectionAsync(cancellationToken);
                await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

                await channel.ExchangeDeclareAsync(Exchange, ExchangeType.Fanout, cancellationToken: cancellationToken);
                await channel.QueueDeclareAsync();
                await channel.QueueBindAsync(QueueName, Exchange, string.Empty, cancellationToken: cancellationToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    messageReceivedCallback(message);
                };

                await channel.BasicConsumeAsync(QueueName, autoAck: true, consumer: consumer);
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe to message queue");
                throw;
            }
        }
    }
}
