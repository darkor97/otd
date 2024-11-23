namespace Handler.Infrastructure.Configuration
{
    internal record struct MongoSettings
    {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
    }
}
