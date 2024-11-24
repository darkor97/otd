using Handler.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Handler.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationExtensions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IOddsService, OddsService>();

            return serviceCollection;
        }
    }
}
