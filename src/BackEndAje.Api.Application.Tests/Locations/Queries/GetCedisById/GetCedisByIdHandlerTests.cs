namespace BackEndAje.Api.Application.Tests.Locations.Queries.GetCedisById
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisById;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetCedisByIdHandlerTests
    {
        private readonly Mock<ICediRepository> _cediRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCedisByIdHandler _handler;

        public GetCedisByIdHandlerTests()
        {
            this._cediRepositoryMock = new Mock<ICediRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetCedisByIdHandler(this._cediRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedResult_WhenCediExists()
        {
            // Arrange
            var cediId = 5;

            var cediEntity = new Cedi
            {
                CediId = cediId,
                CediName = "CEDI Norte",
            };

            var expectedResult = new GetCedisByIdResult
            {
                Id = cediId,
                CediName = "CEDI Norte",
            };

            this._cediRepositoryMock.Setup(r => r.GetCediByCediIdAsync(cediId)).ReturnsAsync(cediEntity);
            this._mapperMock.Setup(m => m.Map<GetCedisByIdResult>(cediEntity)).Returns(expectedResult);

            var query = new GetCedisByIdQuery(cediId);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCediDoesNotExist()
        {
            // Arrange
            var cediId = 99;
            this._cediRepositoryMock.Setup(r => r.GetCediByCediIdAsync(cediId))!.ReturnsAsync((Cedi?)null);
            this._mapperMock.Setup(m => m.Map<GetCedisByIdResult>(null)).Returns(((GetCedisByIdResult?)null) !);

            var query = new GetCedisByIdQuery(cediId);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}

