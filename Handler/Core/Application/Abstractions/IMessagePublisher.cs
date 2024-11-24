namespace Handler.Application.Publish
{
    public interface IMessagePublisher
    {
        Task PublishAsync(string message, CancellationToken cancellationToken = default);
    }
}
