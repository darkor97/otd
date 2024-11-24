namespace Punter.Domain
{
    public record Odds
    {
        public required string HomeTeam;
        public required string AwayTeam;

        public required double HomeOdds;
        public required double AwayOdds;
        public required double DrawOdds;
    }
}
