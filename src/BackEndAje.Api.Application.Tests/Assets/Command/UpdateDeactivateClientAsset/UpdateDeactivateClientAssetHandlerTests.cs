namespace BackEndAje.Api.Application.Tests.Assets.Command.UpdateDeactivateClientAsset
{
    using BackEndAje.Api.Application.Asset.Command.UpdateDeactivateClientAsset;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class UpdateDeactivateClientAssetHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly UpdateDeactivateClientAssetHandler _handler;

        public UpdateDeactivateClientAssetHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._handler = new UpdateDeactivateClientAssetHandler(_clientAssetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAssetDoesNotExist()
        {
            // Arrange
            var command = new UpdateDeactivateClientAssetCommand
            {
                ClientAssetId = 1,
                UpdatedBy = 5,
                Notes = "Motivo",
            };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))!
                .ReturnsAsync((ClientAssets)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cliente con Activo asociado no existe.");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAnotherClientHasActiveCodeAje()
        {
            // Arrange
            var command = new UpdateDeactivateClientAssetCommand
            {
                ClientAssetId = 1,
                UpdatedBy = 5,
                Notes = "Desactivación",
            };

            var existing = new ClientAssets
            {
                ClientAssetId = 1,
                CodeAje = "AJE123",
                IsActive = false,
                ClientId = 999,
                AssetId = 500,
            };

            var otherAssets = new List<ClientAssets>
            {
                new ClientAssets { CodeAje = "AJE123", IsActive = true },
            };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))
                .ReturnsAsync(existing);

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetsByCodeAje(existing.CodeAje))
                .ReturnsAsync(otherAssets);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("No se puede actualizar el estado porque ya hay un cliente activo con el código Aje 'AJE123'.");
        }

        [Fact]
        public async Task Handle_ShouldUpdateClientAsset_WhenValid()
        {
            // Arrange
            var command = new UpdateDeactivateClientAssetCommand
            {
                ClientAssetId = 1,
                UpdatedBy = 5,
                Notes = "Cambio de estado",
            };

            var existing = new ClientAssets
            {
                ClientAssetId = 1,
                CodeAje = "AJE124",
                ClientId = 222,
                AssetId = 999,
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                CreatedBy = 1,
            };

            var sameCodeAjeList = new List<ClientAssets>
            {
                new ClientAssets { CodeAje = "AJE124", IsActive = false },
            };

            ClientAssetsTrace? traceCaptured = null;
            ClientAssets? updatedCaptured = null;

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))
                .ReturnsAsync(existing);

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetsByCodeAje(existing.CodeAje))
                .ReturnsAsync(sameCodeAjeList);

            this._clientAssetRepoMock
                .Setup(repo => repo.AddTraceabilityRecordAsync(It.IsAny<ClientAssetsTrace>()))
                .Callback<ClientAssetsTrace>(t => traceCaptured = t)
                .Returns(Task.CompletedTask);

            this._clientAssetRepoMock
                .Setup(repo => repo.UpdateClientAssetsAsync(It.IsAny<ClientAssets>()))
                .Callback<ClientAssets>(a => updatedCaptured = a)
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            traceCaptured.Should().NotBeNull();
            traceCaptured!.ClientAssetId.Should().Be(command.ClientAssetId);
            traceCaptured.PreviousClientId.Should().Be(existing.ClientId);
            traceCaptured.NewClientId.Should().BeNull();
            traceCaptured.AssetId.Should().Be(existing.AssetId);
            traceCaptured.CodeAje.Should().Be(existing.CodeAje);
            traceCaptured.ChangeReason.Should().Be(command.Notes);
            traceCaptured.CreatedBy.Should().Be(command.UpdatedBy);
            traceCaptured.IsActive.Should().BeTrue();

            updatedCaptured.Should().NotBeNull();
            updatedCaptured!.IsActive.Should().BeTrue();
            updatedCaptured.Notes.Should().Be(command.Notes);
            updatedCaptured.UpdatedBy.Should().Be(command.UpdatedBy);
        }
    }
}