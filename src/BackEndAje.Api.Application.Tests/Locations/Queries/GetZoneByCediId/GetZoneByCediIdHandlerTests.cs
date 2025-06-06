namespace BackEndAje.Api.Application.Tests.Locations.Queries.GetZoneByCediId
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetZoneByCediId;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetZoneByCediIdHandlerTests
    {
        private readonly Mock<IZoneRepository> _zoneRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetZoneByCediIdHandler _handler;

        public GetZoneByCediIdHandlerTests()
        {
            this._zoneRepositoryMock = new Mock<IZoneRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetZoneByCediIdHandler(this._zoneRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WithCediId_ShouldReturnMappedZones()
        {
            // Arrange
            var cediId = 1;

            var zones = new List<Zone>
            {
                new Zone { ZoneId = 1, ZoneName = "Zona A" },
                new Zone { ZoneId = 2, ZoneName = "Zona B" },
            };

            var expected = new List<GetZoneByCediIdResult>
            {
                new GetZoneByCediIdResult { ZoneId = 1, ZoneName = "Zona A" },
                new GetZoneByCediIdResult { ZoneId = 2, ZoneName = "Zona B" },
            };

            this._zoneRepositoryMock.Setup(r => r.GetZonesByCediIdAsync(cediId)).ReturnsAsync(zones);
            this._mapperMock.Setup(m => m.Map<List<GetZoneByCediIdResult>>(zones)).Returns(expected);

            var query = new GetZoneByCediIdQuery { CediId = cediId };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Handle_WithoutCediId_ShouldReturnAllMappedZones()
        {
            // Arrange
            var zones = new List<Zone>
            {
                new Zone { ZoneId = 3, ZoneName = "Zona C" },
            };

            var expected = new List<GetZoneByCediIdResult>
            {
                new GetZoneByCediIdResult { ZoneId = 3, ZoneName = "Zona C" },
            };

            this._zoneRepositoryMock.Setup(r => r.GetAllZones()).ReturnsAsync(zones);
            this._mapperMock.Setup(m => m.Map<List<GetZoneByCediIdResult>>(zones)).Returns(expected);

            var query = new GetZoneByCediIdQuery { CediId = null };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
