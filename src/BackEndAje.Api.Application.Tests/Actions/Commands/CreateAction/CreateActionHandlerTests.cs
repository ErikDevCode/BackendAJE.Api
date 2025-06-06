namespace BackEndAje.Api.Application.Tests.Actions.Commands.CreateAction
{
    using AutoMapper;
    using BackEndAje.Api.Application.Actions.Commands.CreateAction;
    using BackEndAje.Api.Application.Dtos.Actions;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class CreateActionHandlerTests
    {
        private readonly Mock<IActionRepository> _actionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateActionHandler _handler;

        public CreateActionHandlerTests()
        {
            this._actionRepositoryMock = new Mock<IActionRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new CreateActionHandler(this._actionRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenActionNameAlreadyExists()
        {
            // Arrange
            var existingAction = new BackEndAje.Api.Domain.Entities.Action { ActionName = "CrearUsuario" };
            var command = new CreateActionCommand
            {
                Action = new CreateActionDto { ActionName = "CrearUsuario" },
            };

            this._actionRepositoryMock
                .Setup(repo => repo.GetAllActionsAsync())
                .ReturnsAsync([existingAction]);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Action 'CrearUsuario' ya existe.");
        }

        [Fact]
        public async Task Handle_ShouldCreateAction_WhenActionDoesNotExist()
        {
            // Arrange
            var command = new CreateActionCommand
            {
                Action = new CreateActionDto { ActionName = "NuevaAccion" },
            };

            this._actionRepositoryMock
                .Setup(repo => repo.GetAllActionsAsync())
                .ReturnsAsync([new BackEndAje.Api.Domain.Entities.Action { ActionName = "OtraAccion" }]);

            var mappedAction = new BackEndAje.Api.Domain.Entities.Action { ActionName = "NuevaAccion" };

            this._mapperMock
                .Setup(mapper => mapper.Map<BackEndAje.Api.Domain.Entities.Action>(
                    It.Is<CreateActionDto>(dto => dto.ActionName == "NuevaAccion")))
                .Returns(mappedAction);

            this._actionRepositoryMock
                .Setup(repo => repo.AddActionAsync(mappedAction))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            this._actionRepositoryMock.Verify(repo => repo.AddActionAsync(It.IsAny<BackEndAje.Api.Domain.Entities.Action>()), Times.Once);
        }
    }
}

