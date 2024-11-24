using Handler.Infrastructure.Setttings;
using Microsoft.Extensions.Configuration;

namespace Handler.Infrastructure.Configuration
{
    public static class InfrastructureConfiguration
    {
        private const string MongoDb = nameof(MongoDb);
        private const string RabbitMQ = nameof(RabbitMQ);

        private static IConfiguration _configuration;

        static InfrastructureConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            _configuration = builder.Build();
        }

        internal static MongoSettings MongoSettings { get => _configuration.GetSection(MongoDb).Get<MongoSettings>(); }
        internal static RabbitMQSettings RabbitMQSettings { get => _configuration.GetSection(RabbitMQ).Get<RabbitMQSettings>(); }
    }
}
