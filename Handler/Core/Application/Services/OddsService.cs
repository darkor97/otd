using Handler.Application.Publish;
using Handler.Domain;
using Handler.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Handler.Application.Services
{
    internal class OddsService : IOddsService
    {
        private readonly IOddsRepository _oddsRepository;
        private readonly IMessageQueueProvider _messageQueueProvider;
        private readonly ILogger<OddsService> _logger;

        public OddsService(IOddsRepository oddsRepository, IMessageQueueProvider messageQueueProvider, ILogger<OddsService> logger)
        {
            _oddsRepository = oddsRepository;
            _messageQueueProvider = messageQueueProvider;
            _logger = logger;
        }

        public async Task<Odds> CreateOddsAsync(Odds odds, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _oddsRepository.CreateOddsAsync(odds, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create new odds");
                throw;
            }
        }
        public async Task<Odds> UpdateOddsAsync(Odds odds, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _oddsRepository.UpdateOddsAsync(odds, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update odds");
                throw;
            }
        }

        public async Task<IEnumerable<Odds>> GetAllOddsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _oddsRepository.GetAllOddsAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all odds data");
                throw;
            }
        }

        public async Task<Odds> GetOddsAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _oddsRepository.GetOddsAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get odds for id: {id}", id);
                throw;
            }
        }
        public async Task<bool> DeleteOddsAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _oddsRepository.DeleteOddsAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete odds with id: {id}", id);
                throw;
            }
        }

        public async Task PublishAsync(IEnumerable<Odds> oddsToPublish, CancellationToken cancellation = default)
        {
            try
            {
                var oddsToPublishAsString = JsonSerializer.Serialize(oddsToPublish);
                await _messageQueueProvider.PublishAsync(oddsToPublishAsString, cancellation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish odds to consumers");
                throw;
            }
        }
    }
}
