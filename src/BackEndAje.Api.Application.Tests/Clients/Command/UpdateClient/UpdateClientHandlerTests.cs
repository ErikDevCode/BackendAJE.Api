namespace BackEndAje.Api.Application.Tests.Clients.Command.UpdateClient
{
    using AutoFixture;
    using AutoMapper;
    using BackEndAje.Api.Application.Clients.Commands.UpdateClient;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class UpdateClientHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateClientHandler _handler;
        private readonly Fixture _fixture;

        public UpdateClientHandlerTests()
        {
            this._clientRepoMock = new Mock<IClientRepository>();
            this._userRepoMock = new Mock<IUserRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._fixture = new Fixture();
            this._fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => this._fixture.Behaviors.Remove(b));
            this._fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            this._handler = new UpdateClientHandler(this._clientRepoMock.Object, this._mapperMock.Object, this._userRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateClient_WhenClientExists()
        {
            // Arrange
            var command = this._fixture.Build<UpdateClientCommand>()
                .With(c => c.ClientId, 123)
                .With(c => c.Route, 101)
                .Create();

            var existingClient = this._fixture.Build<Client>()
                .With(c => c.ClientId, command.ClientId)
                .With(c => c.CreatedAt, DateTime.UtcNow.AddMonths(-1))
                .With(c => c.CreatedBy, 1)
                .Create();

            var user = this._fixture.Build<User>()
                .With(u => u.Route, command.Route)
                .With(u => u.UserId, 124)
                .Create();

            var mappedClient = this._fixture.Build<Client>()
                .With(c => c.ClientId, command.ClientId)
                .Create();

            this._clientRepoMock.Setup(r => r.GetClientById(command.ClientId))
                .ReturnsAsync(existingClient);

            this._userRepoMock.Setup(r => r.GetUserByRouteAsync(command.Route))
                .ReturnsAsync(user);

            this._mapperMock.Setup(m => m.Map<Client>(command))
                .Returns(mappedClient);

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            result.Should().Be(Unit.Value);
            mappedClient.UserId.Should().Be(user.UserId);
            mappedClient.CreatedAt.Should().Be(existingClient.CreatedAt);
            mappedClient.CreatedBy.Should().Be(existingClient.CreatedBy);

            this._clientRepoMock.Verify(
                r => r.UpdateClientAsync(It.Is<Client>(c => c.ClientId == command.ClientId)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientDoesNotExist()
        {
            // Arrange
            var command = this._fixture.Create<UpdateClientCommand>();

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

