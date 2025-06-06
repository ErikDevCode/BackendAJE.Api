namespace BackEndAje.Api.Application.Tests.Clients.Command.CreateClient
{
    using AutoFixture;
    using AutoMapper;
    using BackEndAje.Api.Application.Clients.Commands.CreateClient;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class CreateClientHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateClientHandler _handler;
        private readonly Fixture _fixture;

        public CreateClientHandlerTests()
        {
            this._clientRepoMock = new Mock<IClientRepository>();
            this._userRepoMock = new Mock<IUserRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new CreateClientHandler(this._clientRepoMock.Object, this._mapperMock.Object, this._userRepoMock.Object);
            this._fixture = new Fixture();
            this._fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => this._fixture.Behaviors.Remove(b));
            this._fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Handle_ShouldCreateClient_WhenDocumentIsUnique()
        {
            // Arrange
            var command = this._fixture.Build<CreateClientCommand>()
                                  .With(c => c.Route, 123)
                                  .With(c => c.DocumentNumber, "12345678")
                                  .Create();

            var user = this._fixture.Build<User>()
                               .With(u => u.Route, 124)
                               .Create();

            var mappedClient = this._fixture.Build<Client>()
                                       .With(c => c.DocumentNumber, command.DocumentNumber)
                                       .Create();

            this._clientRepoMock.Setup(repo => repo.GetClientByDocumentNumber(command.DocumentNumber))
                           .ReturnsAsync((Client?)null);

            this._userRepoMock.Setup(repo => repo.GetUserByRouteAsync(command.Route))
                         .ReturnsAsync(user);

            this._mapperMock.Setup(m => m.Map<Client>(command))
                       .Returns(mappedClient);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            this._clientRepoMock.Verify(r => r.AddClient(It.Is<Client>(c => c.DocumentNumber == command.DocumentNumber)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAlreadyExists()
        {
            // Arrange
            var command = this._fixture.Create<CreateClientCommand>();
            var existingClient = this._fixture.Create<Client>();

            this._clientRepoMock.Setup(repo => repo.GetClientByDocumentNumber(command.DocumentNumber))
                           .ReturnsAsync(existingClient);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Cliente con documento: '{command.DocumentNumber}' ya existe.");

            this._clientRepoMock.Verify(r => r.AddClient(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var command = this._fixture.Build<CreateClientCommand>()
                                  .With(c => c.Route, 123)
                                  .Create();

            this._clientRepoMock.Setup(repo => repo.GetClientByDocumentNumber(command.DocumentNumber))
                           .ReturnsAsync((Client?)null);

            this._userRepoMock.Setup(repo => repo.GetUserByRouteAsync(command.Route))
                         .ReturnsAsync((User?)null);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NullReferenceException>();

            this._clientRepoMock.Verify(r => r.AddClient(It.IsAny<Client>()), Times.Never);
        }
    }
}
