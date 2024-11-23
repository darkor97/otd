using Handler.Domain;

namespace Handler.Application.Services
{
    public interface IOddsService
    {
        Task<Odds> CreateOddsAsync(Odds odds);
        Task<Odds> GetOddsAsync(string id);
        Task<IEnumerable<Odds>> GetAllOddsAsync();
        Task<Odds> UpdateOddsAsync(Odds odds);
        Task<bool> DeleteOddsAsync(string id);
    }
}
