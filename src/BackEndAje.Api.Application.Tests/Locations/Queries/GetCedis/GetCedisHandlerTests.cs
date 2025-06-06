namespace BackEndAje.Api.Application.Tests.Locations.Queries.GetCedis
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetCedis;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetCedisHandlerTests
    {
        private readonly Mock<ICediRepository> _cediRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCedisHandler _handler;

        public GetCedisHandlerTests()
        {
            this._cediRepositoryMock = new Mock<ICediRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetCedisHandler(this._cediRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedCedisList()
        {
            // Arrange
            var cedis = new List<Cedi>
            {
                new Cedi { CediId = 1, CediName = "CEDI Lima" },
                new Cedi { CediId = 2, CediName = "CEDI Arequipa" },
            };

            var expectedResults = new List<GetCedisResult>
            {
                new GetCedisResult { CediId = 1, CediName = "CEDI Lima" },
                new GetCedisResult { CediId = 2, CediName = "CEDI Arequipa" },
            };

            this._cediRepositoryMock.Setup(r => r.GetAllCedis()).ReturnsAsync(cedis);
            this._mapperMock.Setup(m => m.Map<List<GetCedisResult>>(cedis)).Returns(expectedResults);

            var query = new GetCedisQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResults);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCedisFound()
        {
            // Arrange
            this._cediRepositoryMock.Setup(r => r.GetAllCedis()).ReturnsAsync(new List<Cedi>());
            this._mapperMock.Setup(m => m.Map<List<GetCedisResult>>(It.IsAny<List<Cedi>>()))
                       .Returns([]);

            var query = new GetCedisQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
