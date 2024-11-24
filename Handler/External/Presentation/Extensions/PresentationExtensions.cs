using Handler.Presentation.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace Handler.Presentation.Extensions
{
    public static class PresentationExtensions
    {
        public static IServiceCollection AddPresentationExtensions(this IServiceCollection services)
        {
            services
                .AddSingleton<OddsActions>();
            services
                .AddMemoryCache();

            return services;
        }
    }
}
