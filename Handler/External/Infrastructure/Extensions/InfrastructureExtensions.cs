using Handler.Domain.Repositories;
using Handler.Infrastructure.Configuration;
using Handler.Infrastructure.Extensions;
using Handler.Infrastructure.Publish;
using Handler.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace Handler.Infrastructure.Mongo
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddHandlerInfrastructureExtensions(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                return new MongoClient(InfrastructureConfiguration.MongoSettings.ConnectionString);
            });

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(InfrastructureConfiguration.MongoSettings.DatabaseName);
                database.MapMongoEntities();
                return database;
            });

            services.AddSingleton<IConnectionFactory>(servicesProvider =>
            {
                return new ConnectionFactory()
                {
                    HostName = InfrastructureConfiguration.RabbitMQSettings.HostName,
                    Port = InfrastructureConfiguration.RabbitMQSettings.Port,
                    UserName = InfrastructureConfiguration.RabbitMQSettings.UserName,
                    Password = InfrastructureConfiguration.RabbitMQSettings.Password,
                    VirtualHost = InfrastructureConfiguration.RabbitMQSettings.VirtualHost,
                };
            });

            services.AddScoped<IOddsRepository, OddsRepository>();
            services.AddScoped<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}
