namespace Handler.Domain
{
    public record Odds
    {
        public string Id { get; private set; }

        public required string HomeTeam { get; init; }
        public required string AwayTeam { get; init; }

        public required double HomeOdds { get; set; }
        public required double AwayOdds { get; set; }
        public required double DrawOdds { get; set; }
    }
}
