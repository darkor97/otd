using Handler.Domain;
using Handler.Domain.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Handler.Infrastructure.Repositories
{
    internal class OddsRepository : IOddsRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly ILogger<OddsRepository> _logger;

        public OddsRepository(IMongoDatabase mongoDatabase, ILogger<OddsRepository> logger)
        {
            _mongoDatabase = mongoDatabase;
            _logger = logger;
        }

        public async Task<Odds> CreateOddsAsync(Odds odds, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = GetCollection();
                await collection.InsertOneAsync(odds, cancellationToken: cancellationToken);
                return odds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create new odds in database");
                throw;
            }
        }

        public async Task<IEnumerable<Odds>> GetAllOddsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = GetCollection();
                return await collection.Find(_ => true).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all odds from database");
                throw;
            }
        }

        public async Task<Odds> GetOddsAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var collections = GetCollection();
                return await collections.Find(x => x.Id == id).FirstAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get get odds for id: {id}", id);
                throw;
            }
        }

        public async Task<Odds> UpdateOddsAsync(Odds odds, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = GetCollection();
                var result = await collection.ReplaceOneAsync(x => x.Id == odds.Id, odds, cancellationToken: cancellationToken);
                if (result.IsAcknowledged && result.MatchedCount > 0 && result.ModifiedCount > 0)
                {
                    return odds;
                }
                else
                {
                    throw new Exception("Cannot update odds, object does not exist in the database");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update odds");
                throw;
            }

        }

        public async Task<bool> DeleteOddsAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = GetCollection();
                var result = await collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
                return result.IsAcknowledged;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deleted odds");
                throw;
            }
        }

        private IMongoCollection<Odds> GetCollection() => _mongoDatabase.GetCollection<Odds>(nameof(Odds));
    }
}
