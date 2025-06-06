namespace BackEndAje.Api.Application.Tests.Assets.Command.DeleteClientAsset
{
    using BackEndAje.Api.Application.Asset.Command.DeleteClientAsset;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class DeleteClientAssetHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly DeleteClientAssetHandler _handler;

        public DeleteClientAssetHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._handler = new DeleteClientAssetHandler(this._clientAssetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAssetDoesNotExist()
        {
            // Arrange
            var command = new DeleteClientAssetCommand { ClientAssetId = 123 };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))!
                .ReturnsAsync((ClientAssets)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Client con Activo asociado no existe.");

            this._clientAssetRepoMock.Verify(repo => repo.DeleteClientAssetAsync(It.IsAny<ClientAssets>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteClientAsset_WhenExists()
        {
            // Arrange
            var command = new DeleteClientAssetCommand { ClientAssetId = 124 };
            var existingClientAsset = new ClientAssets { ClientAssetId = command.ClientAssetId };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))
                .ReturnsAsync(existingClientAsset);

            this._clientAssetRepoMock
                .Setup(repo => repo.DeleteClientAssetAsync(existingClientAsset))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            this._clientAssetRepoMock.Verify(repo => repo.DeleteClientAssetAsync(existingClientAsset), Times.Once);
        }
    }
}

