namespace Handler.Application.Publish
{
    public interface IMessageQueueProvider
    {
        Task PublishAsync(string message, CancellationToken cancellationToken = default);
        Task<string> SubscribeAsync(CancellationToken cancellationToken = default);
    }
}
