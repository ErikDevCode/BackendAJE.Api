namespace BackEndAje.Api.Application.Tests.Assets.Queries.GetClientAssets
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssets;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetClientAssetsHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetClientAssetsHandler _handler;

        public GetClientAssetsHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetClientAssetsHandler(this._mapperMock.Object, this._clientAssetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedClientAssets_WhenFiltersMatch()
        {
            // Arrange
            var clientAssets = new List<ClientAssetsDto>
            {
                new ClientAssetsDto { CodeAje = "AJE001", ClientId = 1 },
                new ClientAssetsDto { CodeAje = "AJE002", ClientId = 2 },
            };

            var mappedResult = new List<GetClientAssetsResult>
            {
                new GetClientAssetsResult { CodeAje = "AJE001", ClientId = 1 },
                new GetClientAssetsResult { CodeAje = "AJE002", ClientId = 2 },
            };

            var query = new GetClientAssetsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                CodeAje = "AJE",
                ClientId = 1,
                userId = 2,
                CediId = 3,
                RegionId = 4,
                Route = 11,
                ClientCode = 123,
            };

            this._clientAssetRepoMock
                .Setup(r => r.GetClientAssetsAsync(query.PageNumber, query.PageSize, query.CodeAje, query.ClientId, query.userId, query.CediId, query.RegionId, query.Route, query.ClientCode))
                .ReturnsAsync(clientAssets);

            this._mapperMock
                .Setup(m => m.Map<List<GetClientAssetsResult>>(clientAssets))
                .Returns(mappedResult);

            this._clientAssetRepoMock
                .Setup(r => r.GetTotalClientAssets(query.CodeAje, query.ClientId, query.userId, query.CediId, query.RegionId, query.Route, query.ClientCode))
                .ReturnsAsync(2);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(2);
            result.Items.Should().BeEquivalentTo(mappedResult);
            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            this._clientAssetRepoMock.Verify(
                r =>
                r.GetClientAssetsAsync(query.PageNumber, query.PageSize, query.CodeAje, query.ClientId, query.userId, query.CediId, query.RegionId, query.Route, query.ClientCode), Times.Once);

            this._clientAssetRepoMock.Verify(
                r =>
                r.GetTotalClientAssets(query.CodeAje, query.ClientId, query.userId, query.CediId, query.RegionId, query.Route, query.ClientCode), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldApplyDefaultPagination_WhenPageNumberOrSizeIsNull()
        {
            // Arrange
            var clientAssets = new List<ClientAssetsDto>
            {
                new ClientAssetsDto { CodeAje = "AJE003", ClientId = 3 },
            };

            var mappedResult = new List<GetClientAssetsResult>
            {
                new GetClientAssetsResult { CodeAje = "AJE003", ClientId = 3 },
            };

            var query = new GetClientAssetsQuery
            {
                PageNumber = 1,
                PageSize = 1,
                CodeAje = "AJE003",
            };

            this._clientAssetRepoMock
                .Setup(r => r.GetClientAssetsAsync(1, 1, "AJE003", null, null, null, null, null, null))
                .ReturnsAsync(clientAssets);

            this._mapperMock
                .Setup(m => m.Map<List<GetClientAssetsResult>>(clientAssets))
                .Returns(mappedResult);

            this._clientAssetRepoMock
                .Setup(r => r.GetTotalClientAssets("AJE003", null, null, null, null, null, null))
                .ReturnsAsync(1);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(1);
            result.TotalCount.Should().Be(1);
            result.Items.Should().BeEquivalentTo(mappedResult);
        }
    }
}

