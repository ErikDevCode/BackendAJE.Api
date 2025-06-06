namespace BackEndAje.Api.Application.Tests.Assets.Command.CreateClientAsset
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Command.CreateClientAsset;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class CreateClientAssetHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly Mock<IAssetRepository> _assetRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateClientAssetHandler _handler;

        public CreateClientAssetHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._assetRepoMock = new Mock<IAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new CreateClientAssetHandler(this._mapperMock.Object, this._clientAssetRepoMock.Object, this._assetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAssetAlreadyExists()
        {
            // Arrange
            var command = new CreateClientAssetCommand { CodeAje = "AJE001" };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetsByCodeAje(command.CodeAje))
                .ReturnsAsync(new List<ClientAssets> { new ClientAssets() });

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage(
                    $"Codigo Aje: '{command.CodeAje}' tiene un Cliente vigente asignado actulmente, primero debe desasignar el cliente anterior");

            this._assetRepoMock.Verify(x => x.GetAssetByCodeAje(It.IsAny<string>()), Times.Never);
            this._clientAssetRepoMock.Verify(x => x.AddClientAsset(It.IsAny<ClientAssets>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAssetDoesNotExist()
        {
            // Arrange
            var command = new CreateClientAssetCommand { CodeAje = "AJE002" };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetsByCodeAje(command.CodeAje))
                .ReturnsAsync([]);

            this._assetRepoMock
                .Setup(repo => repo.GetAssetByCodeAje(command.CodeAje))
                .ReturnsAsync([]);

            // Act
            Func<Task> act = async () => await this._handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage(
                    $"Codigo Aje: '{command.CodeAje}' no existe en los registros de Activos debe cargarlo antes de relacionarlo.");

            this._clientAssetRepoMock.Verify(x => x.AddClientAsset(It.IsAny<ClientAssets>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCreateClientAsset_WhenValid()
        {
            // Arrange
            var command = new CreateClientAssetCommand { CodeAje = "AJE003" };

            this._clientAssetRepoMock
                .Setup(repo => repo.GetClientAssetsByCodeAje(command.CodeAje))
                .ReturnsAsync([]);

            var assetList = new List<Domain.Entities.Asset>
            {
                new Domain.Entities.Asset { AssetId = 123, IsActive = true },
                new Domain.Entities.Asset { AssetId = 124, IsActive = false },
            };

            this._assetRepoMock
                .Setup(repo => repo.GetAssetByCodeAje(command.CodeAje))
                .ReturnsAsync(assetList);

            var mappedClientAsset = new ClientAssets();
            this._mapperMock
                .Setup(mapper => mapper.Map<ClientAssets>(command))
                .Returns(mappedClientAsset);

            this._clientAssetRepoMock
                .Setup(repo => repo.AddClientAsset(mappedClientAsset))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            mappedClientAsset.AssetId.Should().Be(assetList.First(x => x.IsActive).AssetId);
            this._clientAssetRepoMock.Verify(x => x.AddClientAsset(mappedClientAsset), Times.Once);
        }
    }
}

