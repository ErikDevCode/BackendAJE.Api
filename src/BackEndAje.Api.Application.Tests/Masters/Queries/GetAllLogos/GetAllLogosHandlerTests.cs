using AutoMapper;
using BackEndAje.Api.Application.Masters.Queries.GetAllLogos;
using BackEndAje.Api.Domain.Entities;
using BackEndAje.Api.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllLogos
{
    public class GetAllLogosHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllLogosHandler _handler;

        public GetAllLogosHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllLogosHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedLogos()
        {
            // Arrange
            var logos = new List<Logo>
            {
                new Logo { LogoId = 1, LogoDescription = "https://example.com/logo1.png" },
                new Logo { LogoId = 2, LogoDescription = "https://example.com/logo2.png" },
            };

            var expected = new List<GetAllLogosResult>
            {
                new GetAllLogosResult { LogoId = 1, LogoDescription = "https://example.com/logo1.png" },
                new GetAllLogosResult { LogoId = 2, LogoDescription = "https://example.com/logo2.png" },
            };

            this._mastersRepositoryMock.Setup(r => r.GetAllLogos()).ReturnsAsync(logos);
            this._mapperMock.Setup(m => m.Map<List<GetAllLogosResult>>(logos)).Returns(expected);

            var query = new GetAllLogosQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}

