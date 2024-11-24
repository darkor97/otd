using Handler.Application.Extensions;
using Handler.Infrastructure.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Punter.Presentation.Actions;

namespace Punter.Presentation
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
               .AddInfrastructureExtensions()
               .AddApplicationExtensions()
               .AddMemoryCache()
               .AddLogging(builder => builder.AddConsole())
               .AddSingleton<OddsActions>()
               .BuildServiceProvider();

            var oddsActions = serviceProvider.GetRequiredService<OddsActions>();

            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Punter View");

                await oddsActions.SubscribeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error {ex.Message}, shutting down");
            }
        }
    }
}
