namespace BackEndAje.Api.Application.Tests.OrderRequestAsssets.Command.DeleteAssetToOrderRequest
{
    using AutoFixture;
    using BackEndAje.Api.Application.OrderRequestAssets.Commands.DeleteAssetToOrderRequest;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class DeleteAssetToOrderRequestHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock;
        private readonly Mock<IClientAssetRepository> _clientAssetRepositoryMock;
        private readonly DeleteAssetToOrderRequestHandler _handler;
        private readonly Fixture _fixture;

        public DeleteAssetToOrderRequestHandlerTests()
        {
            this._orderRequestRepositoryMock = new Mock<IOrderRequestRepository>();
            this._clientAssetRepositoryMock = new Mock<IClientAssetRepository>();
            this._handler = new DeleteAssetToOrderRequestHandler(this._orderRequestRepositoryMock.Object, this._clientAssetRepositoryMock.Object);
            this._fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_ShouldDeleteAssetAndAddTrace_WhenClientAssetIsNotActive()
        {
            // Arrange
            var request = new DeleteAssetToOrderRequestCommand
            {
                OrderRequestAssetId = 1,
                AssignedBy = 1,
            };

            var orderRequestAsset = new Domain.Entities.OrderRequestAssets
            {
                OrderRequestAssetId = 1,
                OrderRequestId = 10,
                AssetId = 5,
                IsActive = true,
            };

            var orderRequest = new OrderRequest { OrderRequestId = 10, ClientId = 99 };

            this._orderRequestRepositoryMock.Setup(x => x.GetOrderRequestAssetsById(request.OrderRequestAssetId))
                .ReturnsAsync(orderRequestAsset);

            this._orderRequestRepositoryMock.Setup(x => x.GetOrderRequestById(orderRequestAsset.OrderRequestId))
                .ReturnsAsync(orderRequest);

            this._clientAssetRepositoryMock.Setup(x => x.GetClientAssetByClientIdAndAssetIdAndIsNotActivateAsync(orderRequest.ClientId, orderRequestAsset.AssetId))!
                .ReturnsAsync((ClientAssets?)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            this._orderRequestRepositoryMock.Verify(x => x.UpdateAssetToOrderRequest(It.Is<Domain.Entities.OrderRequestAssets>(o => o.IsActive == false)), Times.Once);
            this._orderRequestRepositoryMock.Verify(x => x.AddOrderRequestAssetTrace(It.Is<OrderRequestAssetsTrace>(t => !t.IsActive)), Times.Once);
            this._clientAssetRepositoryMock.Verify(x => x.DeleteClientAssetAsync(It.IsAny<ClientAssets>()), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenClientAssetExists()
        {
            // Arrange
            var request = new DeleteAssetToOrderRequestCommand
            {
                OrderRequestAssetId = 2,
                AssignedBy = 2,
            };

            var orderRequestAsset = new Domain.Entities.OrderRequestAssets
            {
                OrderRequestAssetId = 2,
                OrderRequestId = 20,
                AssetId = 15,
                IsActive = true,
            };

            var orderRequest = new OrderRequest { OrderRequestId = 20, ClientId = 88 };

            var existingClientAsset = new ClientAssets();

            this._orderRequestRepositoryMock.Setup(x => x.GetOrderRequestAssetsById(request.OrderRequestAssetId))
                .ReturnsAsync(orderRequestAsset);

            this._orderRequestRepositoryMock.Setup(x => x.GetOrderRequestById(orderRequestAsset.OrderRequestId))
                .ReturnsAsync(orderRequest);

            this._clientAssetRepositoryMock.Setup(x => x.GetClientAssetByClientIdAndAssetIdAndIsNotActivateAsync(orderRequest.ClientId, orderRequestAsset.AssetId))
                .ReturnsAsync(existingClientAsset);

            // Act
            Func<Task> act = async () => await this._handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No se puede eliminar el Activo porque ya esta atendido.");
        }
    }
}

