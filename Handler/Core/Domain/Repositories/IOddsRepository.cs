namespace Handler.Domain.Repositories
{
    public interface IOddsRepository
    {
        Task<Odds> CreateOddsAsync(Odds odds);
        Task<Odds> GetOddsAsync(string id);
        Task<IEnumerable<Odds>> GetAllOddsAsync();
        Task<Odds> UpdateOddsAsync(Odds odds);
        Task<bool> DeleteOddsAsync(string id);
    }
}
