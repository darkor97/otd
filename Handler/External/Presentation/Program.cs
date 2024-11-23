using Handler.Application.Extensions;
using Handler.Infrastructure.Mongo;
using Handler.Presentation.Actions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Handler.Presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddHandlerInfrastructureExtensions()
                .AddHandlerApplicationExtensions()
                .AddHandlerPresentationExtensions()
                .AddLogging(builder => builder.AddConsole())
                .BuildServiceProvider();

            var oddsActions = serviceProvider.GetRequiredService<OddsActions>();

            Console.WriteLine("Handler UI");

            while (true)
            {
                oddsActions.ShowInstructions();
                Console.WriteLine("Enter command (ignores case)");
                var userCommand = Console.ReadLine();

                switch (userCommand.ToUpperInvariant())
                {
                    case "C":
                        await oddsActions.CreateAsync();
                        break;
                    case "A":
                        await oddsActions.PrintAllAsync();
                        break;
                    case "U":
                        await oddsActions.PrintAllAsync();
                        break;
                    case "D":
                        await oddsActions.PrintAllAsync();
                        break;
                    case "S":
                        Console.WriteLine("Stopping process");
                        return;
                    default:
                        Console.WriteLine("Unrecognized command");
                        break;
                }
            }
        }
    }
}
