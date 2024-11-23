using Handler.Application.Services;
using Handler.Domain;
using System.Globalization;

namespace Handler.Presentation.Actions
{
    public class OddsActions
    {
        private readonly IOddsService _oddsService;

        public OddsActions(IOddsService oddsService) => _oddsService = oddsService;

        public void ShowInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Available commands");
            Console.WriteLine("For input: C");
            Console.WriteLine("Action is to create new odds");
            Console.WriteLine("For input: A");
            Console.WriteLine("Action is to list all odds");
            Console.WriteLine("For input: U");
            Console.WriteLine("Action is to update odds");
            Console.WriteLine("For input: D");
            Console.WriteLine("Action is to delete odds");
            Console.WriteLine("For input: P");
            Console.WriteLine("Action is to publish odds");
            Console.WriteLine("For input: S");
            Console.WriteLine("Action is to stop the process");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public async Task CreateAsync()
        {
            Console.Clear();
            Console.WriteLine("Odds creations, please insert values as they are required");
            Console.WriteLine("----------------------------");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify Home Team");
            Console.ForegroundColor = ConsoleColor.White;
            var homeTeam = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify Away Team");
            Console.ForegroundColor = ConsoleColor.White;
            var awayTeam = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify Home Odds");
            Console.ForegroundColor = ConsoleColor.White;
            double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var homeOdds);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify Away Odds");
            Console.ForegroundColor = ConsoleColor.White;
            double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var awayOdds);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify Draw Odds");
            Console.ForegroundColor = ConsoleColor.White;
            double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var drawOdds);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Creating odds for match: {homeTeam} vs {awayTeam}");
            Console.WriteLine("Home win -- draw -- Away win");
            Console.WriteLine($"{homeOdds} -- {drawOdds} -- {awayOdds}");
            Console.ForegroundColor = ConsoleColor.White;

            var odds = new Odds()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeOdds = homeOdds,
                DrawOdds = drawOdds,
                AwayOdds = awayOdds,
            };

            await _oddsService.CreateOddsAsync(odds);

            Console.WriteLine("Odds successfully created, returning..");
            Thread.Sleep(1500);
            Console.Clear();
        }

        public async Task PrintAllAsync()
        {
            var odds = await _oddsService.GetAllOddsAsync();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All odds overview");
            Console.WriteLine();
            Console.WriteLine("Home | Away | Home Win | Draw | Away Win} ");
            Console.WriteLine("-----------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var odd in odds)
            {
                Console.WriteLine($"{odd.HomeTeam} vs {odd.AwayTeam}: {odd.HomeOdds} - {odd.DrawOdds} - {odd.AwayOdds}");
                Console.WriteLine("-----------------");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
