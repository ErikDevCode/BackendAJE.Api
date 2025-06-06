namespace BackEndAje.Api.Application.Tests.Asset.Queries.GetAssetsByCodeAje
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Queries.GetAssetsByCodeAje;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAssetsByCodeAjeHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAssetsByCodeAjeHandler _handler;

        public GetAssetsByCodeAjeHandlerTests()
        {
            this._assetRepoMock = new Mock<IAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAssetsByCodeAjeHandler(this._assetRepoMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedAssets_WhenFound()
        {
            // Arrange
            var codeAje = "AJE123";
            var assets = new List<Domain.Entities.Asset>
            {
                new Domain.Entities.Asset { CodeAje = codeAje, Brand = "Marca1" },
                new Domain.Entities.Asset { CodeAje = codeAje, Brand = "Marca2" },
            };

            var expectedResult = new List<GetAssetsByCodeAjeResult>
            {
                new GetAssetsByCodeAjeResult { CodeAje = codeAje, Brand = "Marca1" },
                new GetAssetsByCodeAjeResult { CodeAje = codeAje, Brand = "Marca2" },
            };

            this._assetRepoMock.Setup(r => r.GetAssetByCodeAje(codeAje)).ReturnsAsync(assets);
            this._mapperMock.Setup(m => m.Map<List<GetAssetsByCodeAjeResult>>(assets)).Returns(expectedResult);

            var query = new GetAssetsByCodeAjeQuery(codeAje);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            this._assetRepoMock.Verify(r => r.GetAssetByCodeAje(codeAje), Times.Once);
            this._mapperMock.Verify(m => m.Map<List<GetAssetsByCodeAjeResult>>(assets), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenNoAssetsFound()
        {
            // Arrange
            var codeAje = "NOTFOUND";
            this._assetRepoMock.Setup(r => r.GetAssetByCodeAje(codeAje)).ReturnsAsync(new List<Domain.Entities.Asset>());

            var query = new GetAssetsByCodeAjeQuery(codeAje);

            // Act
            Func<Task> act = async () => await this._handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Activo con código {codeAje} no encontrado.");

            this._assetRepoMock.Verify(r => r.GetAssetByCodeAje(codeAje), Times.Once);
            this._mapperMock.Verify(m => m.Map<List<GetAssetsByCodeAjeResult>>(It.IsAny<List<Domain.Entities.Asset>>()), Times.Never);
        }
    }
}
