namespace BackEndAje.Api.Application.Tests.Assets.Queries.GetAssetWithOutClient
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Queries.GetAssetWithOutClient;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAssetWithOutClientHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAssetWithOutClientHandler _handler;

        public GetAssetWithOutClientHandlerTests()
        {
            this._assetRepoMock = new Mock<IAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAssetWithOutClientHandler(this._assetRepoMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedResult_WhenAssetsExist()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var codeAje = "AJE001";

            var mockAssets = new List<Domain.Entities.Asset>
            {
                new Domain.Entities.Asset { CodeAje = "AJE001", Brand = "Marca A" },
                new Domain.Entities.Asset { CodeAje = "AJE001", Brand = "Marca B" },
            };

            var expectedMappedResult = new List<GetAssetWithOutClientResult>
            {
                new GetAssetWithOutClientResult { CodeAje = "AJE001", Brand = "Marca A" },
                new GetAssetWithOutClientResult { CodeAje = "AJE001", Brand = "Marca B" },
            };

            this._assetRepoMock
                .Setup(r => r.GetAssetsWithOutClient(pageNumber, pageSize, codeAje))
                .ReturnsAsync(mockAssets);

            this._assetRepoMock
                .Setup(r => r.GetTotalAssetsWithOutClient(codeAje))
                .ReturnsAsync(2);

            this._mapperMock
                .Setup(m => m.Map<List<GetAssetWithOutClientResult>>(mockAssets))
                .Returns(expectedMappedResult);

            var query = new GetAssetWithOutClientQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                CodeAje = codeAje,
            };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(2);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.Items.Should().BeEquivalentTo(expectedMappedResult);

            this._assetRepoMock.Verify(r => r.GetAssetsWithOutClient(pageNumber, pageSize, codeAje), Times.Once);
            this._assetRepoMock.Verify(r => r.GetTotalAssetsWithOutClient(codeAje), Times.Once);
            this._mapperMock.Verify(m => m.Map<List<GetAssetWithOutClientResult>>(mockAssets), Times.Once);
        }
    }
}

