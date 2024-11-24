using Handler.Application.Publish;
using Handler.Application.Services;
using Handler.Domain;
using Handler.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Handler.Tests.Application
{
    [TestClass]
    public class OddsServiceTests
    {
        private Mock<IOddsRepository> _oddsRepositoryMock;
        private Mock<IMessageQueueProvider> _messageQueueProviderMock;
        private Mock<ILogger<OddsService>> _loggerMock;

        private IOddsService _sut;

        [TestInitialize]
        public void Initialize()
        {
            _oddsRepositoryMock = new Mock<IOddsRepository>();
            SetupRepository();
            _messageQueueProviderMock = new Mock<IMessageQueueProvider>();
            _loggerMock = new Mock<ILogger<OddsService>>();

            _sut = new OddsService(_oddsRepositoryMock.Object, _messageQueueProviderMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task CreateOddsAsync_ShouldExecute()
        {
            var result = await _sut.CreateOddsAsync(CreateMockOdds().First());

            Assert.IsNotNull(result);

            VerifyCreateOddsAsync(Times.Once);
        }

        [TestMethod]
        public async Task GetAllOddsAsync_ShouldExecute()
        {
            var result = await _sut.GetAllOddsAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            CollectionAssert.AllItemsAreNotNull(result.ToArray());

            VerifyGetAllOddsAsync(Times.Once);
        }

        [TestMethod]
        public async Task UpdateOddsAsync_ShouldThrowException()
        {
            await Assert.ThrowsExceptionAsync<Exception>(() => _sut.UpdateOddsAsync(CreateMockOdds().First()));
        }

        #region Setup

        private void SetupRepository()
        {
            _oddsRepositoryMock
                .Setup(x => x.CreateOddsAsync(It.IsAny<Odds>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Odds x, CancellationToken token) => { return x; });

            _oddsRepositoryMock
                .Setup(x => x.GetAllOddsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateMockOdds());

            _oddsRepositoryMock
                .Setup(x => x.UpdateOddsAsync(It.IsAny<Odds>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Testing message"));
        }

        #endregion

        #region MockData

        private IEnumerable<Odds> CreateMockOdds()
        {
            return new List<Odds>()
            {
                new Odds() { HomeTeam = "Home", AwayTeam = "Away", HomeOdds = 1, AwayOdds = 1, DrawOdds = 1},
                new Odds() { HomeTeam = "Home", AwayTeam = "Away", HomeOdds = 1, AwayOdds = 1, DrawOdds = 1},
                new Odds() { HomeTeam = "Home", AwayTeam = "Away", HomeOdds = 1, AwayOdds = 1, DrawOdds = 1},
            };
        }

        #endregion

        #region Verify

        private void VerifyCreateOddsAsync(Func<Times> times)
        {
            _oddsRepositoryMock
                .Verify(x => x.CreateOddsAsync(It.IsAny<Odds>(), It.IsAny<CancellationToken>()), times);
        }

        private void VerifyGetAllOddsAsync(Func<Times> times)
        {
            _oddsRepositoryMock
                .Verify(x => x.GetAllOddsAsync(It.IsAny<CancellationToken>()), times);
        }

        #endregion
    }
}
