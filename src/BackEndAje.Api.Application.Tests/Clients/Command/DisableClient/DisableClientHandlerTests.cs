namespace BackEndAje.Api.Application.Tests.Clients.Command.DisableClient
{
    using AutoFixture;
    using BackEndAje.Api.Application.Clients.Commands.DisableClient;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class DisableClientHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock;
        private readonly DisableClientHandler _handler;
        private readonly Fixture _fixture;

        public DisableClientHandlerTests()
        {
            this._clientRepoMock = new Mock<IClientRepository>();
            this._handler = new DisableClientHandler(this._clientRepoMock.Object);
            this._fixture = new Fixture();
            this._fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => this._fixture.Behaviors.Remove(b));
            this._fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldDisableClient_WhenClientExists()
        {
            // Arrange
            var command = this._fixture.Build<DisableClientCommand>()
                .With(c => c.ClientId, 123)
                .With(c => c.UpdatedBy, 1)
                .Create();

            var client = this._fixture.Build<Client>()
                .With(c => c.ClientId, command.ClientId)
                .With(c => c.IsActive, true)
                .Create();

            this._clientRepoMock.Setup(r => r.GetClientById(command.ClientId))
                .ReturnsAsync(client);

            this._clientRepoMock.Setup(r => r.UpdateClientAsync(It.IsAny<Client>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
            client.IsActive.Should().BeFalse(); // Se desactivó correctamente
            client.UpdatedBy.Should().Be(command.UpdatedBy);
            this._clientRepoMock.Verify(r => r.UpdateClientAsync(It.Is<Client>(c => c.ClientId == command.ClientId)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientDoesNotExist()
        {
            // Arrange
            var command = this._fixture.Create<DisableClientCommand>();

            this._clientRepoMock.Setup(r => r.GetClientById(command.ClientId))
                .ReturnsAsync((Client?)null);

            // Act
            var act = async () => await this._handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Cliente con ID '{command.ClientId}' no existe.");
        }
    }
}

