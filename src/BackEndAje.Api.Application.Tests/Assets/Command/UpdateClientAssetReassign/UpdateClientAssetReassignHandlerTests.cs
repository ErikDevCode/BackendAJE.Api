namespace BackEndAje.Api.Application.Tests.Assets.Command.UpdateClientAssetReassign
{
    using BackEndAje.Api.Application.Asset.Command.UpdateClientAssetReassign;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class UpdateClientAssetReassignHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly UpdateClientAssetReassignHandler _handler;

        public UpdateClientAssetReassignHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._handler = new UpdateClientAssetReassignHandler(this._clientAssetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAssetNotFound()
        {
            // Arrange
            var command = new UpdateClientAssetReassignCommand { ClientAssetId = 123 };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))!
                .ReturnsAsync((ClientAssets)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("El activo con el cliente asociado no existe.");

            this._clientAssetRepoMock.Verify(repo => repo.UpdateClientAssetsAsync(It.IsAny<ClientAssets>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAssetIsAlreadyActive()
        {
            // Arrange
            var command = new UpdateClientAssetReassignCommand { ClientAssetId = 123};

            var existing = new ClientAssets
            {
                ClientAssetId = command.ClientAssetId,
                IsActive = true,
            };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))
                .ReturnsAsync(existing);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("No se puede reasignar un Activo que está activo. Debe desactivarse primero.");

            this._clientAssetRepoMock.Verify(repo => repo.UpdateClientAssetsAsync(It.IsAny<ClientAssets>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReassignClientAsset_WhenValid()
        {
            // Arrange
            var command = new UpdateClientAssetReassignCommand
            {
                ClientAssetId = 123,
                NewClientId = 111,
                Notes = "Reasignación por cambio de tienda",
                UpdatedBy = 12,
            };

            var existing = new ClientAssets
            {
                ClientAssetId = command.ClientAssetId,
                AssetId = 124,
                CodeAje = "AJE001",
                ClientId = 111,
                IsActive = null,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                CreatedBy = 2,
            };

            ClientAssetsTrace? traceCaptured = null;
            ClientAssets? updatedAsset = null;

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetByIdAsync(command.ClientAssetId))
                .ReturnsAsync(existing);

            this._clientAssetRepoMock
                .Setup(repo => repo.AddTraceabilityRecordAsync(It.IsAny<ClientAssetsTrace>()))
                .Callback<ClientAssetsTrace>(trace => traceCaptured = trace)
                .Returns(Task.CompletedTask);

            this._clientAssetRepoMock
                .Setup(repo => repo.UpdateClientAssetsAsync(It.IsAny<ClientAssets>()))
                .Callback<ClientAssets>(asset => updatedAsset = asset)
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            // Verifica trazabilidad
            traceCaptured.Should().NotBeNull();
            traceCaptured!.ClientAssetId.Should().Be(command.ClientAssetId);
            traceCaptured.PreviousClientId.Should().Be(existing.ClientId);
            traceCaptured.NewClientId.Should().Be(command.NewClientId);
            traceCaptured.AssetId.Should().Be(existing.AssetId);
            traceCaptured.CodeAje.Should().Be(existing.CodeAje);
            traceCaptured.ChangeReason.Should().Be(command.Notes);
            traceCaptured.CreatedBy.Should().Be(command.UpdatedBy);
            traceCaptured.IsActive.Should().BeTrue();

            // Verifica actualización del asset
            updatedAsset.Should().NotBeNull();
            updatedAsset!.ClientId.Should().Be(command.NewClientId);
            updatedAsset.Notes.Should().Be(command.Notes);
            updatedAsset.UpdatedBy.Should().Be(command.UpdatedBy);
            updatedAsset.IsActive.Should().BeTrue();
        }
    }
}

