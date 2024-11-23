using Handler.Domain;
using Handler.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Handler.Application.Services
{
    public class OddsService : IOddsService
    {
        private readonly IOddsRepository _oddsRepository;
        private readonly ILogger<OddsService> _logger;

        public OddsService(IOddsRepository oddsRepository, ILogger<OddsService> logger)
        {
            _oddsRepository = oddsRepository;
            _logger = logger;
        }

        public async Task<Odds> CreateOddsAsync(Odds odds)
        {
            try
            {
                return await _oddsRepository.CreateOddsAsync(odds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create new odds");
                throw;
            }
        }
        public async Task<Odds> UpdateOddsAsync(Odds odds)
        {
            try
            {
                return await _oddsRepository.UpdateOddsAsync(odds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update odds");
                throw;
            }
        }

        public async Task<IEnumerable<Odds>> GetAllOddsAsync()
        {
            try
            {
                return await _oddsRepository.GetAllOddsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all odds data");
                throw;
            }
        }

        public async Task<Odds> GetOddsAsync(string id)
        {
            try
            {
                return await _oddsRepository.GetOddsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get odds for id: {id}", id);
                throw;
            }
        }
        public async Task<bool> DeleteOddsAsync(string id)
        {
            try
            {
                return await _oddsRepository.DeleteOddsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete odds with id: {id}", id);
                throw;
            }
        }
    }
}
