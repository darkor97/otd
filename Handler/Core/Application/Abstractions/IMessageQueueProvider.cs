namespace Handler.Application.Publish
{
    public interface IMessageQueueProvider
    {
        Task PublishAsync(string message, CancellationToken cancellationToken = default);
        Task SubscribeAsync(Action<string> messageReceivedCallback, CancellationToken cancellationToken = default);
    }
}
