using Handler.Domain.Repositories;
using Handler.Infrastructure.Configuration;
using Handler.Infrastructure.Extensions;
using Handler.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

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

            services.AddScoped<IOddsRepository, OddsRepository>();

            return services;
        }
    }
}
