namespace BackEndAje.Api.Application.Tests.Assets.Command.UpdateAsset
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Command.UpdateAsset;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class UpdateAssetHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateAssetHandler _handler;

        public UpdateAssetHandlerTests()
        {
            this._assetRepositoryMock = new Mock<IAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new UpdateAssetHandler(this._assetRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAssetByIdNotFound()
        {
            // Arrange
            var command = new UpdateAssetCommand { AssetId = 123, CodeAje = "AJE001" };

            this._assetRepositoryMock
                .Setup(repo => repo.GetAssetById(command.AssetId))
                .ReturnsAsync((Domain.Entities.Asset)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"Activo con ID '{command.AssetId}' no existe.");

            this._assetRepositoryMock.Verify(repo => repo.UpdateAssetAsync(It.IsAny<Domain.Entities.Asset>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAssetWithSameCodeAjeAlreadyExists()
        {
            // Arrange
            var command = new UpdateAssetCommand
            {
                AssetId = 123,
                CodeAje = "AJE001",
            };

            var existingAssetById = new Domain.Entities.Asset
            {
                AssetId = command.AssetId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
            };

            var conflictAssets = new List<Domain.Entities.Asset> { new Domain.Entities.Asset { CodeAje = "AJE001" } };

            this._assetRepositoryMock
                .Setup(repo => repo.GetAssetById(command.AssetId))
                .ReturnsAsync(existingAssetById);

            this._mapperMock
                .Setup(mapper => mapper.Map<Domain.Entities.Asset>(command))
                .Returns(new Domain.Entities.Asset { CodeAje = "AJE001" });

            this._assetRepositoryMock
                .Setup(repo => repo.GetAssetByCodeAje(command.CodeAje))
                .ReturnsAsync(conflictAssets);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Activo ya se encuentra registrado.");

            this._assetRepositoryMock.Verify(repo => repo.UpdateAssetAsync(It.IsAny<Domain.Entities.Asset>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateAsset_WhenValid()
        {
            // Arrange
            var command = new UpdateAssetCommand
            {
                AssetId = 123,
                CodeAje = "AJE002",
            };

            var existingAssetById = new Domain.Entities.Asset
            {
                AssetId = command.AssetId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
            };

            this._assetRepositoryMock
                .Setup(repo => repo.GetAssetById(command.AssetId))
                .ReturnsAsync(existingAssetById);

            var mappedAsset = new Domain.Entities.Asset { CodeAje = "AJE002" };

            this._mapperMock
                .Setup(mapper => mapper.Map<Domain.Entities.Asset>(command))
                .Returns(mappedAsset);

            this._assetRepositoryMock
                .Setup(repo => repo.GetAssetByCodeAje(command.CodeAje))
                .ReturnsAsync([]);

            this._assetRepositoryMock
                .Setup(repo => repo.UpdateAssetAsync(mappedAsset))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            mappedAsset.CreatedAt.Should().Be(existingAssetById.CreatedAt);
            mappedAsset.CreatedBy.Should().Be(existingAssetById.CreatedBy);
            this._assetRepositoryMock.Verify(repo => repo.UpdateAssetAsync(mappedAsset), Times.Once);
        }
    }
}