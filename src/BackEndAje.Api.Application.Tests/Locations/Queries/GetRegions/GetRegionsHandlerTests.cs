namespace BackEndAje.Api.Application.Tests.Locations.Queries.GetRegions
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetRegions;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetRegionsHandlerTests
    {
        private readonly Mock<IRegionRepository> _regionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetRegionsHandler _handler;

        public GetRegionsHandlerTests()
        {
            this._regionRepositoryMock = new Mock<IRegionRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetRegionsHandler(this._regionRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedRegions_WhenRegionsExist()
        {
            // Arrange
            var regions = new List<Region>
            {
                new Region { RegionId = 1, RegionName = "Lima" },
                new Region { RegionId = 2, RegionName = "Arequipa" },
            };

            var expected = new List<GetRegionsResult>
            {
                new GetRegionsResult { RegionId = 1, RegionName = "Lima" },
                new GetRegionsResult { RegionId = 2, RegionName = "Arequipa" },
            };

            this._regionRepositoryMock.Setup(r => r.GetAllRegionsAsync()).ReturnsAsync(regions);
            this._mapperMock.Setup(m => m.Map<List<GetRegionsResult>>(regions)).Returns(expected);

            var query = new GetRegionsQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoRegionsExist()
        {
            // Arrange
            this._regionRepositoryMock.Setup(r => r.GetAllRegionsAsync()).ReturnsAsync([]);
            this._mapperMock.Setup(m => m.Map<List<GetRegionsResult>>(It.IsAny<List<Region>>()))
                       .Returns([]);

            var query = new GetRegionsQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
