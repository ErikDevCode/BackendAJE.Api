namespace BackEndAje.Api.Application.Tests.Assets.Command.UpdateStatusAsset
{
    using BackEndAje.Api.Application.Asset.Command.UpdateStatusAsset;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class UpdateStatusAssetHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepoMock;
        private readonly UpdateStatusAssetHandler _handler;

        public UpdateStatusAssetHandlerTests()
        {
            this._assetRepoMock = new Mock<IAssetRepository>();
            this._handler = new UpdateStatusAssetHandler(this._assetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAssetDoesNotExist()
        {
            // Arrange
            var command = new UpdateStatusAssetCommand
            {
                AssetId = 123,
                UpdatedBy = 99,
            };

            this._assetRepoMock
                .Setup(repo => repo.GetAssetById(command.AssetId))
                .ReturnsAsync((Domain.Entities.Asset)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Activo con ID '{command.AssetId}' no existe.");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_ShouldToggleIsActive_AndUpdateAsset(bool initialIsActive)
        {
            // Arrange
            var command = new UpdateStatusAssetCommand
            {
                AssetId = 123,
                UpdatedBy = 42,
            };

            var asset = new Domain.Entities.Asset
            {
                AssetId = command.AssetId,
                IsActive = initialIsActive,
                UpdatedBy = 0,
            };

            this._assetRepoMock
                .Setup(repo => repo.GetAssetById(command.AssetId))
                .ReturnsAsync(asset);

            Domain.Entities.Asset? updatedAsset = null;

            this._assetRepoMock
                .Setup(repo => repo.UpdateAssetAsync(It.IsAny<Domain.Entities.Asset>()))
                .Callback<Domain.Entities.Asset>(a => updatedAsset = a)
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            updatedAsset.Should().NotBeNull();
            updatedAsset!.UpdatedBy.Should().Be(command.UpdatedBy);
            var expected = !initialIsActive;
            updatedAsset.IsActive.Should().Be(expected);
        }
    }
}
