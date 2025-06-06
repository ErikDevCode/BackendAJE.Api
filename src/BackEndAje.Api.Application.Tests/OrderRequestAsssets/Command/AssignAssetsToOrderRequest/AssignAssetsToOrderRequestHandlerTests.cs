namespace BackEndAje.Api.Application.Tests.OrderRequestAsssets.Command.AssignAssetsToOrderRequest
{
    using BackEndAje.Api.Application.OrderRequestAssets.Commands.AssignAssetsToOrderRequest;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class AssignAssetsToOrderRequestHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepoMock;
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly Mock<IAssetRepository> _assetRepoMock;
        private readonly AssignAssetsToOrderRequestHandler _handler;

        public AssignAssetsToOrderRequestHandlerTests()
        {
            this._orderRequestRepoMock = new Mock<IOrderRequestRepository>();
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._assetRepoMock = new Mock<IAssetRepository>();

            this._handler = new AssignAssetsToOrderRequestHandler(
                this._orderRequestRepoMock.Object,
                this._clientAssetRepoMock.Object,
                this._assetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAssignAssetsAndCreateTraces()
        {
            // Arrange
            var orderRequestId = 1;
            var userId = 99;
            var assetIds = new List<int> { 10, 20 };
            var orderRequest = new OrderRequest { OrderRequestId = orderRequestId, CediId = 1, ClientId = 2 };

            this._orderRequestRepoMock.Setup(r => r.GetOrderRequestById(orderRequestId))
                .ReturnsAsync(orderRequest);

            foreach (var assetId in assetIds)
            {
                var asset = new Domain.Entities.Asset { AssetId = assetId, CodeAje = $"Code-{assetId}" };

                this._assetRepoMock.Setup(r => r.GetAssetById(assetId)).ReturnsAsync(asset);
                this._orderRequestRepoMock.Setup(r => r.AssignAssetToOrder(orderRequestId, assetId, userId))
                    .ReturnsAsync(assetId * 100); // Simulated OrderRequestAssetId

                this._clientAssetRepoMock.Setup(r => r.AddClientAsset(It.IsAny<ClientAssets>())).Returns(Task.CompletedTask);
                this._orderRequestRepoMock.Setup(r => r.AddOrderRequestAssetTrace(It.IsAny<OrderRequestAssetsTrace>()))
                    .Returns(Task.CompletedTask);
            }

            var command = new AssignAssetsToOrderRequestCommand
            {
                OrderRequestId = orderRequestId,
                AssignedBy = userId,
                AssetIds = assetIds,
            };

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            this._orderRequestRepoMock.Verify(r => r.GetOrderRequestById(orderRequestId), Times.Once);
            foreach (var assetId in assetIds)
            {
                this._assetRepoMock.Verify(r => r.GetAssetById(assetId), Times.Once);
                this._orderRequestRepoMock.Verify(r => r.AssignAssetToOrder(orderRequestId, assetId, userId), Times.Once);
                this._clientAssetRepoMock.Verify(r => r.AddClientAsset(It.Is<ClientAssets>(a => a.AssetId == assetId)), Times.Once);
                this._orderRequestRepoMock.Verify(r => r.AddOrderRequestAssetTrace(It.Is<OrderRequestAssetsTrace>(t => t.AssetId == assetId)), Times.Once);
            }
        }
    }
}

