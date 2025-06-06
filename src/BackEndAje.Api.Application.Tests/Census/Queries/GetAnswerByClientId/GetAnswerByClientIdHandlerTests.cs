namespace BackEndAje.Api.Application.Tests.Census.Queries.GetAnswerByClientId
{
    using AutoFixture;
    using AutoMapper;
    using BackEndAje.Api.Application.Census.Queries.GetAnswerByClientId;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAnswerByClientIdHandlerTests
    {
        private readonly Mock<ICensusRepository> _censusRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAnswerByClientIdHandler _handler;
        private readonly Fixture _fixture;

        public GetAnswerByClientIdHandlerTests()
        {
            this._censusRepoMock = new Mock<ICensusRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAnswerByClientIdHandler(this._censusRepoMock.Object, this._mapperMock.Object);
            this._fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedResult_WhenDataExists()
        {
            // Arrange
            var clientId = 123;
            var assetId = 124;
            var monthPeriod = "202406";
            var pageNumber = 1;
            var pageSize = 10;

            var query = new GetAnswerByClientIdQuery
            {
                ClientId = clientId,
                AssetId = assetId,
                MonthPeriod = monthPeriod,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var mockEntities = this._fixture.CreateMany<ClientAssetWithCensusAnswersDto>().ToList();
            var totalCount = mockEntities.Count;
            var expectedMapped = this._fixture.CreateMany<ClientAssetWithCensusAnswersDto>(totalCount).ToList();

            this._censusRepoMock
                .Setup(repo => repo.GetClientAssetsWithCensusAnswersAsync(pageNumber, pageSize, assetId, clientId, monthPeriod))
                .ReturnsAsync((mockEntities, totalCount));

            this._mapperMock
                .Setup(m => m.Map<List<ClientAssetWithCensusAnswersDto>>(mockEntities))
                .Returns(expectedMapped);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(totalCount);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.Items.Should().BeEquivalentTo(expectedMapped);

            this._censusRepoMock.Verify(repo => repo.GetClientAssetsWithCensusAnswersAsync(pageNumber, pageSize, assetId, clientId, monthPeriod), Times.Once);
            this._mapperMock.Verify(m => m.Map<List<ClientAssetWithCensusAnswersDto>>(mockEntities), Times.Once);
        }
    }
}

