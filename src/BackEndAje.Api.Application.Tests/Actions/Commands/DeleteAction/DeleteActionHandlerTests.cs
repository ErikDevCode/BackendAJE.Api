namespace BackEndAje.Api.Application.Tests.Actions.Commands.DeleteAction
{
    using BackEndAje.Api.Application.Actions.Commands.DeleteAction;
    using BackEndAje.Api.Application.Dtos.Actions;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class DeleteActionHandlerTests
    {
        private readonly Mock<IActionRepository> _actionRepositoryMock;
        private readonly DeleteActionHandler _handler;

        public DeleteActionHandlerTests()
        {
            this._actionRepositoryMock = new Mock<IActionRepository>();
            this._handler = new DeleteActionHandler(this._actionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteAction_WhenActionExists()
        {
            // Arrange
            const int actionId = 123;
            var existingAction = new BackEndAje.Api.Domain.Entities.Action { ActionId = actionId };

            var command = new DeleteActionCommand
            {
                DeleteAction = new DeleteActionDto { ActionId = actionId },
            };

            this._actionRepositoryMock
                .Setup(repo => repo.GetActionByIdAsync(actionId))
                .ReturnsAsync(existingAction);

            this._actionRepositoryMock
                .Setup(repo => repo.DeleteActionAsync(existingAction))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            this._actionRepositoryMock.Verify(repo => repo.DeleteActionAsync(existingAction), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenActionDoesNotExist()
        {
            // Arrange
            const int actionId = 123;

            var command = new DeleteActionCommand
            {
                DeleteAction = new DeleteActionDto { ActionId = actionId },
            };

            this._actionRepositoryMock
                .Setup(repo => repo.GetActionByIdAsync(actionId))
                .ReturnsAsync((BackEndAje.Api.Domain.Entities.Action)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"ActionId '{actionId}' no existe.");

            this._actionRepositoryMock.Verify(repo => repo.DeleteActionAsync(It.IsAny<BackEndAje.Api.Domain.Entities.Action>()), Times.Never);
        }
    }
}