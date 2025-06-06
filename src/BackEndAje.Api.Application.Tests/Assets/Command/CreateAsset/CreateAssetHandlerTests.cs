namespace BackEndAje.Api.Application.Tests.Assets.Command.CreateAsset
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Command.CreateAsset;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class CreateAssetHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateAssetHandler _handler;

        public CreateAssetHandlerTests()
        {
            this._assetRepositoryMock = new Mock<IAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new CreateAssetHandler(this._assetRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateAsset_WhenAssetDoesNotExist()
        {
            // Arrange
            const string assetType = "EEFF ACTUAL";
            var command = new CreateAssetCommand
            {
                CodeAje = "123",
                Logo = "LogoX",
            };

            this._assetRepositoryMock
                .Setup(repo => repo.GetAssetByCodeAjeAndLogoAndAssetType(command.CodeAje, command.Logo, assetType))
                .ReturnsAsync((Domain.Entities.Asset)null!);

            var mappedAsset = new Domain.Entities.Asset { CodeAje = "123", Logo = "LogoX" };

            this._mapperMock
                .Setup(m => m.Map<Domain.Entities.Asset>(command))
                .Returns(mappedAsset);

            this._assetRepositoryMock
                .Setup(repo => repo.AddAsset(mappedAsset))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            mappedAsset.AssetType.Should().Be(assetType);
            this._assetRepositoryMock.Verify(repo => repo.AddAsset(mappedAsset), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAssetAlreadyExists()
        {
            // Arrange
            const string assetType = "EEFF ACTUAL";
            var command = new CreateAssetCommand
            {
                CodeAje = "123",
                Logo = "LogoX",
            };

            var existingAsset = new Domain.Entities.Asset { CodeAje = "123", Logo = "LogoX", AssetType = assetType };

            this._assetRepositoryMock
                .Setup(repo => repo.GetAssetByCodeAjeAndLogoAndAssetType(command.CodeAje, command.Logo, assetType))
                .ReturnsAsync(existingAsset);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"Activo '{command.CodeAje}' ya existe actualmente.");

            this._assetRepositoryMock.Verify(repo => repo.AddAsset(It.IsAny<Domain.Entities.Asset>()), Times.Never);
        }
    }
}

