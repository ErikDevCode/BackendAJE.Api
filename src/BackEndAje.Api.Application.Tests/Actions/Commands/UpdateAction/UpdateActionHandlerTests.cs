namespace BackEndAje.Api.Application.Tests.Actions.Commands.UpdateAction
{
    using AutoMapper;
    using BackEndAje.Api.Application.Actions.Commands.UpdateAction;
    using BackEndAje.Api.Application.Dtos.Actions;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class UpdateActionHandlerTests
    {
        private readonly Mock<IActionRepository> _actionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateActionHandler _handler;

        public UpdateActionHandlerTests()
        {
            this._actionRepositoryMock = new Mock<IActionRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new UpdateActionHandler(this._actionRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateAction_WhenActionExists()
        {
            // Arrange
            const int actionId = 123;

            var existingAction = new BackEndAje.Api.Domain.Entities.Action { ActionId = actionId, ActionName = "AntiguoNombre" };

            var command = new UpdateActionCommand
            {
                Action = new UpdateActionDto
                {
                    ActionId = actionId,
                    ActionName = "NuevoNombre",
                },
            };

            this._actionRepositoryMock
                .Setup(repo => repo.GetAllActionsAsync())
                .ReturnsAsync([existingAction]);

            this._mapperMock
                .Setup(m => m.Map(command.Action, existingAction));

            this._actionRepositoryMock
                .Setup(repo => repo.UpdateActionAsync(existingAction))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            this._mapperMock.Verify(m => m.Map(command.Action, existingAction), Times.Once);
            this._actionRepositoryMock.Verify(repo => repo.UpdateActionAsync(existingAction), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenActionDoesNotExist()
        {
            // Arrange
            var command = new UpdateActionCommand
            {
                Action = new UpdateActionDto
                {
                    ActionId = 123,
                    ActionName = "NoExiste",
                },
            };

            this._actionRepositoryMock
                .Setup(repo => repo.GetAllActionsAsync())
                .ReturnsAsync([]);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"Action con ID '{command.Action.ActionId}' no existe.");

            this._mapperMock.Verify(m => m.Map(It.IsAny<UpdateActionDto>(), It.IsAny<Action>()), Times.Never);
            this._actionRepositoryMock.Verify(repo => repo.UpdateActionAsync(It.IsAny<BackEndAje.Api.Domain.Entities.Action>()), Times.Never);
        }
    }
}