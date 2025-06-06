namespace BackEndAje.Api.Application.Tests.Assets.Command.UpdateClientAsset
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Command.UpdateClientAsset;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class UpdateClientAssetHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateClientAssetHandler _handler;

        public UpdateClientAssetHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new UpdateClientAssetHandler(this._mapperMock.Object, this._clientAssetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAssetDoesNotExist()
        {
            // Arrange
            var command = new UpdateClientAssetCommand { ClientAssetId = 123 };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))!
                .ReturnsAsync((ClientAssets)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Client con Activo asociado no existe.");

            this._clientAssetRepoMock.Verify(repo => repo.UpdateClientAssetsAsync(It.IsAny<ClientAssets>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateClientAsset_WhenExists()
        {
            // Arrange
            var command = new UpdateClientAssetCommand
            {
                ClientAssetId = 123,
                Notes = "NuevoValor",
            };

            var existingClientAsset = new ClientAssets
            {
                ClientAssetId = command.ClientAssetId,
                AssetId = 123,
                CodeAje = "AJE001",
                ClientId = 111,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                CreatedBy = 1,
            };

            var mappedClientAsset = new ClientAssets
            {
                ClientAssetId = command.ClientAssetId,
                Notes = "NuevoValor",
            };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))
                .ReturnsAsync(existingClientAsset);

            this._mapperMock
                .Setup(mapper => mapper.Map<ClientAssets>(command))
                .Returns(mappedClientAsset);

            this._clientAssetRepoMock
                .Setup(repo => repo.UpdateClientAssetsAsync(mappedClientAsset))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            mappedClientAsset.AssetId.Should().Be(existingClientAsset.AssetId);
            mappedClientAsset.CodeAje.Should().Be(existingClientAsset.CodeAje);
            mappedClientAsset.ClientId.Should().Be(existingClientAsset.ClientId);
            mappedClientAsset.IsActive.Should().BeTrue();
            mappedClientAsset.CreatedAt.Should().Be(existingClientAsset.CreatedAt);
            mappedClientAsset.CreatedBy.Should().Be(existingClientAsset.CreatedBy);

            this._clientAssetRepoMock.Verify(repo => repo.UpdateClientAssetsAsync(mappedClientAsset), Times.Once);
        }
    }
}

