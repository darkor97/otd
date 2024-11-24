namespace Handler.Domain.Repositories
{
    public interface IOddsRepository
    {
        Task<Odds> CreateOddsAsync(Odds odds, CancellationToken cancellationToken = default);
        Task<Odds> GetOddsAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Odds>> GetAllOddsAsync(CancellationToken cancellationToken = default);
        Task<Odds> UpdateOddsAsync(Odds odds, CancellationToken cancellationToken = default);
        Task<bool> DeleteOddsAsync(string id, CancellationToken cancellationToken = default);
    }
}
