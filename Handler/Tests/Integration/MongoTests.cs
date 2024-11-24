using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Handler.Tests.Integration
{
    [TestClass]
    public class MongoTests
    {
        public const string ConnectionString = "mongodb://localhost:27017";
        public const string Database = "Test";
        public const string Collection = "test-collection";

        private static MongoClient _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new MongoClient(ConnectionString);
        }

        [TestMethod]
        public async Task Mongo_CanPull()
        {
            var database = _sut.GetDatabase(Database);
            var collection = database.GetCollection<object>(Collection);

            var items = await collection.Find(_ => true).ToListAsync();

            Assert.IsNotNull(collection);
            Assert.IsFalse(items.Any());
        }

        [TestMethod]
        public async Task Mongo_CanInsert()
        {
            var database = _sut.GetDatabase(Database);
            var collection = database.GetCollection<object>(Collection);

            await collection.InsertOneAsync(new object());
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            var database = _sut.GetDatabase(Database);
            await database.DropCollectionAsync(Collection);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _sut.DropDatabase(Database);
        }
    }
}
