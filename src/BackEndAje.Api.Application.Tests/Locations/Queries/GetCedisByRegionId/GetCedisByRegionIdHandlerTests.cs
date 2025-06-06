namespace BackEndAje.Api.Application.Tests.Locations.Queries.GetCedisByRegionId
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisByRegionId;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetCedisByRegionIdHandlerTests
    {
        private readonly Mock<ICediRepository> _cediRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCedisByRegionIdHandler _handler;

        public GetCedisByRegionIdHandlerTests()
        {
            this._cediRepositoryMock = new Mock<ICediRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetCedisByRegionIdHandler(this._cediRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedList_WhenCedisExist()
        {
            // Arrange
            var regionId = 1;

            var cediEntities = new List<Cedi>
            {
                new Cedi { CediId = 1, CediName = "CEDI Lima" },
                new Cedi { CediId = 2, CediName = "CEDI Norte" },
            };

            var expectedResults = new List<GetCedisByRegionIdResult>
            {
                new GetCedisByRegionIdResult { CediId = 1, CediName = "CEDI Lima" },
                new GetCedisByRegionIdResult { CediId = 2, CediName = "CEDI Norte" },
            };

            this._cediRepositoryMock.Setup(r => r.GetCedisByRegionIdAsync(regionId)).ReturnsAsync(cediEntities);
            this._mapperMock.Setup(m => m.Map<List<GetCedisByRegionIdResult>>(cediEntities)).Returns(expectedResults);

            var query = new GetCedisByRegionIdQuery { RegionId = regionId };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedResults);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCedisFound()
        {
            // Arrange
            var regionId = 999;

            this._cediRepositoryMock.Setup(r => r.GetCedisByRegionIdAsync(regionId)).ReturnsAsync(new List<Cedi>());
            this._mapperMock.Setup(m => m.Map<List<GetCedisByRegionIdResult>>(It.IsAny<List<Cedi>>())).Returns(new List<GetCedisByRegionIdResult>());

            var query = new GetCedisByRegionIdQuery{ RegionId = regionId };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
