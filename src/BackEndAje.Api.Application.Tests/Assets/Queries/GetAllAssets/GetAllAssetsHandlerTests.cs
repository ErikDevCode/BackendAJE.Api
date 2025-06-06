namespace BackEndAje.Api.Application.Tests.Assets.Queries.GetAllAssets
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Queries.GetAllAssets;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllAssetsHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllAssetsHandler _handler;

        public GetAllAssetsHandlerTests()
        {
            this._assetRepoMock = new Mock<IAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllAssetsHandler(this._assetRepoMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedAssets()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var codeAjeFilter = "AJE";

            var assetList = new List<Domain.Entities.Asset>
            {
                new Domain.Entities.Asset { CodeAje = "AJE001", Brand = "Marca1" },
                new Domain.Entities.Asset { CodeAje = "AJE002", Brand = "Marca2" },
            };

            var expectedResultList = new List<GetAllAssetsResult>
            {
                new GetAllAssetsResult { CodeAje = "AJE001", Brand = "Marca1" },
                new GetAllAssetsResult { CodeAje = "AJE002", Brand = "Marca2" },
            };

            this._assetRepoMock
                .Setup(repo => repo.GetAssets(pageNumber, pageSize, codeAjeFilter))
                .ReturnsAsync(assetList);

            this._assetRepoMock
                .Setup(repo => repo.GetTotalAssets(codeAjeFilter))
                .ReturnsAsync(2);

            this._mapperMock
                .Setup(m => m.Map<List<GetAllAssetsResult>>(assetList))
                .Returns(expectedResultList);

            var query = new GetAllAssetsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                CodeAje = codeAjeFilter,
            };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(2);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.Items.Should().BeEquivalentTo(expectedResultList);

            this._assetRepoMock.Verify(r => r.GetAssets(pageNumber, pageSize, codeAjeFilter), Times.Once);
            this._assetRepoMock.Verify(r => r.GetTotalAssets(codeAjeFilter), Times.Once);
            this._mapperMock.Verify(m => m.Map<List<GetAllAssetsResult>>(assetList), Times.Once);
        }
    }
}

