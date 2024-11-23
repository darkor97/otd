using Handler.Presentation.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace Handler.Presentation
{
    public static class HandlerPresentationExtensions
    {
        public static IServiceCollection AddHandlerPresentationExtensions(this IServiceCollection services)
        {
            services
                .AddSingleton<OddsActions>();

            return services;
        }
    }
}
