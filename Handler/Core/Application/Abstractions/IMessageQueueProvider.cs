namespace Handler.Application.Publish
{
    public interface IMessageQueueProvider
    {
        Task PublishAsync(string message, CancellationToken cancellationToken = default);
        Task SubscribeAsync(Func<string, Task> messageReceivedCallback, CancellationToken cancellationToken = default);
    }
}
