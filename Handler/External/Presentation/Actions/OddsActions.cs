using Handler.Application.Services;
using Handler.Domain;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace Handler.Presentation.Actions
{
    internal class OddsActions
    {
        private readonly IOddsService _oddsService;
        private readonly IMemoryCache _memoryCache;

        public OddsActions(IOddsService oddsService, IMemoryCache memoryCache)
        {
            _oddsService = oddsService;
            _memoryCache = memoryCache;

            CTS = new CancellationTokenSource();
            SetCallCancelOnConsoleExit();
        }

        public CancellationTokenSource CTS { get; init; }

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
            var homeTeam = Console.ReadLine() ?? string.Empty;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please specify Away Team");
            Console.ForegroundColor = ConsoleColor.White;
            var awayTeam = Console.ReadLine() ?? string.Empty;

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
            Console.WriteLine("# | Home | Away | Home Win | Draw | Away Win |");
            Console.WriteLine("-----------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            var i = 1;
            foreach (var odd in odds)
            {
                Console.WriteLine($"{i} | {odd.HomeTeam} vs {odd.AwayTeam}: {odd.HomeOdds} - {odd.DrawOdds} - {odd.AwayOdds}");
                _memoryCache.Set(i++, odd.Id);
                Console.WriteLine("-----------------");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public async Task UpdateAsync()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Enter the row number of match you want to edit (value in # column)");
            Console.ForegroundColor = ConsoleColor.White;
            int.TryParse(Console.ReadLine(), out var matchNumber);

            var id = _memoryCache.Get<string>(matchNumber);
            if (!string.IsNullOrEmpty(id))
            {
                var odds = await _oddsService.GetOddsAsync(id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Match to update is {odds.HomeTeam} vs {odds.AwayTeam}");
                Console.WriteLine($"Set new odds (blank for no change)");
                Console.WriteLine("Set home odds");
                Console.ForegroundColor = ConsoleColor.White;
                if (double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var homeOdds))
                {
                    odds.HomeOdds = homeOdds;
                }
                else
                {
                    Console.WriteLine("Blank or no input, odds will not change");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Set away odds");
                Console.ForegroundColor = ConsoleColor.White;
                if (double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var awayOdds))
                {
                    odds.AwayOdds = awayOdds;
                }
                else
                {
                    Console.WriteLine("Blank or no input, odds will not change");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Set draw odds");
                Console.ForegroundColor = ConsoleColor.White;
                if (double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var drawOdds))
                {
                    odds.DrawOdds = drawOdds;
                }
                else
                {
                    Console.WriteLine("Blank or no input, odds will not change");
                }

                await _oddsService.UpdateOddsAsync(odds);
                Console.WriteLine("Odds updated successfully, returning..");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else
            {
                Console.WriteLine($"No game with number: {matchNumber} exists");
                Console.Clear();
            }
        }

        public async Task DeleteAsync()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Enter the row number of match you want to delete (value in # column)");
            Console.ForegroundColor = ConsoleColor.White;

            if (int.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out var number))
            {
                if (_memoryCache.TryGetValue(number, out string? id) && !string.IsNullOrEmpty(id))
                {
                    if (await _oddsService.DeleteOddsAsync(id))
                    {
                        Console.WriteLine($"Odds number: {number} deleted, returning..");
                        Thread.Sleep(1500);
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine($"Odds number: {number} failed to be deleted, try again with a different number");
                    }
                }
            }
        }

        public async Task PublishAsync()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Odds to update");

                await _oddsService.PublishAsync(null, CTS.Token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetCallCancelOnConsoleExit()
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                CTS.Cancel();
            };
        }
    }
}
