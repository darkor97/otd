namespace Handler.Infrastructure.Setttings
{
    internal record struct MongoSettings
    {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
    }

    internal record struct RabbitMQSettings
    {
        public string HostName { get; init; }
        public int Port { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
        public string VirtualHost { get; init; }
    }
}
