namespace BackEndAje.Api.Application.Tests.Locations.Queries.GetCedisByUserId
{
    using AutoMapper;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisByUserId;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetCedisByUserIdHandlerTests
    {
        private readonly Mock<ICediRepository> _cediRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCedisByUserIdHandler _handler;

        public GetCedisByUserIdHandlerTests()
        {
            this._cediRepositoryMock = new Mock<ICediRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetCedisByUserIdHandler(this._cediRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedList_WhenCedisExist()
        {
            // Arrange
            var userId = 42;

            var cediEntities = new List<Cedi>
            {
                new Cedi { CediId = 1, CediName = "CEDI Sur" },
                new Cedi { CediId = 2, CediName = "CEDI Norte" },
            };

            var expectedResults = new List<GetCedisByUserIdResult>
            {
                new GetCedisByUserIdResult { CediId = 1, CediName = "CEDI Sur" },
                new GetCedisByUserIdResult { CediId = 2, CediName = "CEDI Norte" },
            };

            this._cediRepositoryMock
                .Setup(r => r.GetCedisByUserIdAsync(userId))
                .ReturnsAsync(cediEntities);

            this._mapperMock
                .Setup(m => m.Map<List<GetCedisByUserIdResult>>(cediEntities))
                .Returns(expectedResults);

            var query = new GetCedisByUserIdQuery(userId);

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
            var userId = 999;

            this._cediRepositoryMock
                .Setup(r => r.GetCedisByUserIdAsync(userId))
                .ReturnsAsync(new List<Cedi>());

            this._mapperMock
                .Setup(m => m.Map<List<GetCedisByUserIdResult>>(It.IsAny<List<Cedi>>()))
                .Returns(new List<GetCedisByUserIdResult>());

            var query = new GetCedisByUserIdQuery(userId);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}

