namespace BackEndAje.Api.Application.Tests.Assets.Queries.GetClientAssetsTrace
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssetsTrace;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetClientAssetsTraceHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly Mock<IClientRepository> _clientRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetClientAssetsTraceHandler _handler;

        public GetClientAssetsTraceHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._clientRepoMock = new Mock<IClientRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetClientAssetsTraceHandler(this._clientAssetRepoMock.Object, this._mapperMock.Object, this._clientRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedTraces_WithClientNames()
        {
            // Arrange
            var assetId = 123;
            var page = 1;
            var size = 10;

            var trace = new ClientAssetsTrace
            {
                ClientAssetId = 124,
                AssetId = assetId,
                PreviousClientId = 100,
                NewClientId = 200,
                CodeAje = "AJE001",
            };

            var mappedResult = new GetClientAssetsTraceResult
            {
                ClientAssetId = trace.ClientAssetId,
                AssetId = trace.AssetId,
                CodeAje = trace.CodeAje,
            };

            this._clientAssetRepoMock
                .Setup(r => r.GetClientAssetTracesByAssetId(page, size, assetId))
                .ReturnsAsync(new List<ClientAssetsTrace> { trace });

            this._clientAssetRepoMock
                .Setup(r => r.GetTotalClientAssetsTrace(assetId))
                .ReturnsAsync(1);

            this._mapperMock
                .Setup(m => m.Map<GetClientAssetsTraceResult>(trace))
                .Returns(mappedResult);

            this._clientRepoMock
                .Setup(r => r.GetClientById(100))
                .ReturnsAsync(new Client { CompanyName = "Cliente Anterior" });

            this._clientRepoMock
                .Setup(r => r.GetClientById(200))
                .ReturnsAsync(new Client { CompanyName = "Cliente Nuevo" });

            var query = new GetClientAssetsTraceQuery
            {
                AssetId = assetId,
                PageNumber = page,
                PageSize = size,
            };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Items.Should().HaveCount(1);
            result.Items[0].PreviousClientName.Should().Be("Cliente Anterior");
            result.Items[0].NewClientName.Should().Be("Cliente Nuevo");
        }

        [Fact]
        public async Task Handle_ShouldUseFallbackName_WhenClientsAreNull()
        {
            // Arrange
            var assetId = 125;

            var trace = new ClientAssetsTrace
            {
                ClientAssetId = 123,
                AssetId = assetId,
                PreviousClientId = null,
                NewClientId = null,
                CodeAje = "AJE002",
            };

            var mappedResult = new GetClientAssetsTraceResult
            {
                ClientAssetId = trace.ClientAssetId,
                AssetId = assetId,
                CodeAje = trace.CodeAje,
            };

            this._clientAssetRepoMock
                .Setup(r => r.GetClientAssetTracesByAssetId(It.IsAny<int>(), It.IsAny<int>(), assetId))
                .ReturnsAsync(new List<ClientAssetsTrace> { trace });

            this._clientAssetRepoMock
                .Setup(r => r.GetTotalClientAssetsTrace(assetId))
                .ReturnsAsync(1);

            this._mapperMock
                .Setup(m => m.Map<GetClientAssetsTraceResult>(trace))
                .Returns(mappedResult);

            var query = new GetClientAssetsTraceQuery
            {
                AssetId = assetId,
                PageNumber = 1,
                PageSize = 10,
            };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Items[0].PreviousClientName.Should().BeNullOrEmpty();
            result.Items[0].NewClientName.Should().BeNullOrEmpty();
        }
    }
}

