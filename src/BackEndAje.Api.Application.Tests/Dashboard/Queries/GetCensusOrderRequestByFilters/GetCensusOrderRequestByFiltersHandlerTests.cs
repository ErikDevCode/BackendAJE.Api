namespace BackEndAje.Api.Application.Tests.Dashboard.Queries.GetCensusOrderRequestByFilters
{
    using BackEndAje.Api.Application.Dashboard.Queries.GetCensusOrderRequestByFilters;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetCensusOrderRequestByFiltersHandlerTests
    {
        private readonly Mock<ICensusRepository> _censusRepoMock;
        private readonly GetCensusOrderRequestByFiltersHandler _handler;

        public GetCensusOrderRequestByFiltersHandlerTests()
        {
            this._censusRepoMock = new Mock<ICensusRepository>();
            this._handler = new GetCensusOrderRequestByFiltersHandler(this._censusRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCensusCount()
        {
            // Arrange
            var expectedCount = 7;
            var query = new GetCensusOrderRequestByFiltersQuery(
                regionId: 1,
                cediId: 2,
                zoneId: 3,
                route: 4,
                month: 5,
                year: 2025
            )
            {
                UserId = 10,
            };

            this._censusRepoMock
                .Setup(r => r.GetCensusCountAsync(
                    query.regionId,
                    query.cediId,
                    query.zoneId,
                    query.route,
                    query.month,
                    query.year,
                    query.UserId))
                .ReturnsAsync(expectedCount);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Type.Should().Be("Censos");
            result.Value.Should().Be(expectedCount);
        }
    }
}

