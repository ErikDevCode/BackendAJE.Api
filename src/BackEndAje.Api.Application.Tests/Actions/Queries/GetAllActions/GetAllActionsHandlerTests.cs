namespace BackEndAje.Api.Application.Tests.Actions.Queries.GetAllActions
{
    using AutoMapper;
    using BackEndAje.Api.Application.Actions.Queries.GetAllActions;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllActionsHandlerTests
    {
        private readonly Mock<IActionRepository> _actionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllActionsHandler _handler;

        public GetAllActionsHandlerTests()
        {
            this._actionRepositoryMock = new Mock<IActionRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllActionsHandler(this._actionRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedActions()
        {
            // Arrange
            var actions = new List<BackEndAje.Api.Domain.Entities.Action>
            {
                new BackEndAje.Api.Domain.Entities.Action { ActionName = "CrearUsuario" },
                new BackEndAje.Api.Domain.Entities.Action { ActionName = "EliminarUsuario" },
            };

            var expectedResult = new List<GetAllActionsResult>
            {
                new GetAllActionsResult { ActionName = "CrearUsuario" },
                new GetAllActionsResult { ActionName = "EliminarUsuario" },
            };

            this._actionRepositoryMock
                .Setup(repo => repo.GetAllActionsAsync())
                .ReturnsAsync(actions);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllActionsResult>>(actions))
                .Returns(expectedResult);

            var query = new GetAllActionsQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            this._actionRepositoryMock.Verify(repo => repo.GetAllActionsAsync(), Times.Once);
            this._mapperMock.Verify(mapper => mapper.Map<List<GetAllActionsResult>>(actions), Times.Once);
        }
    }
}

