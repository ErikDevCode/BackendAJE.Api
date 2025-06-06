namespace BackEndAje.Api.Application.Tests.OrderRequests.Command.DeleteOrderRequest
{
    using BackEndAje.Api.Application.Hubs;
    using BackEndAje.Api.Application.OrderRequests.Commands.DeleteOrderRequest;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;
    using Moq;

    public class DeleteOrderRequestHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock;
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly Mock<IHubContext<NotificationHub>> _hubContextMock;
        private readonly Mock<IClientAssetRepository> _clientAssetRepositoryMock;
        private readonly DeleteOrderRequestHandler _handler;

        public DeleteOrderRequestHandlerTests()
        {
            this._orderRequestRepositoryMock = new Mock<IOrderRequestRepository>();
            this._notificationRepositoryMock = new Mock<INotificationRepository>();
            this._hubContextMock = new Mock<IHubContext<NotificationHub>>();
            this._clientAssetRepositoryMock = new Mock<IClientAssetRepository>();

            var clientsMock = new Mock<IClientProxy>();
            var clientsCallerMock = new Mock<IHubClients>();
            clientsCallerMock.Setup(x => x.User(It.IsAny<string>())).Returns(clientsMock.Object);
            this._hubContextMock.Setup(x => x.Clients).Returns(clientsCallerMock.Object);

            this._handler = new DeleteOrderRequestHandler(
                this._orderRequestRepositoryMock.Object,
                this._notificationRepositoryMock.Object,
                this._hubContextMock.Object,
                this._clientAssetRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_OrderRequest_NotFound()
        {
            // Arrange
            var command = new DeleteOrderRequestCommand { OrderRequestId = 99 };
            this._orderRequestRepositoryMock.Setup(x => x.GetOrderRequestById(99)).ReturnsAsync((OrderRequest?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => this._handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_Should_Delete_OrderRequest_And_Notify_Supervisor()
        {
            // Arrange
            var command = new DeleteOrderRequestCommand { OrderRequestId = 1, UpdatedBy = 100 };

            var orderRequest = new OrderRequest
            {
                OrderRequestId = 1,
                ClientId = 1,
                SupervisorId = 10,
                Client = new Client { ClientCode = 1234 },
                OrderRequestAssets = new List<Domain.Entities.OrderRequestAssets>
                {
                    new Domain.Entities.OrderRequestAssets { OrderRequestAssetId = 1, OrderRequestId = 1, AssetId = 5 },
                },
            };

            this._orderRequestRepositoryMock.Setup(x => x.GetOrderRequestById(1)).ReturnsAsync(orderRequest);
            this._orderRequestRepositoryMock.Setup(x => x.GetListRelocationRequestByOrderRequestId(1)).ReturnsAsync([]);
            this._clientAssetRepositoryMock.Setup(x => x.GetClientAssetByClientIdAndAssetId(1, 5))
                .ReturnsAsync(new ClientAssets());

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            result.Should().Be(Unit.Value);
            this._orderRequestRepositoryMock.Verify(x => x.DeleteOrderRequestAsync(It.Is<OrderRequest>(o => o.OrderRequestId == 1)), Times.Once);
            this._notificationRepositoryMock.Verify(x => x.AddNotificationAsync(It.IsAny<Domain.Entities.Notification>()), Times.Once);
            this._hubContextMock.Verify(x => x.Clients.User("10"), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Delete_RelocationRequests_If_Exist()
        {
            // Arrange
            var command = new DeleteOrderRequestCommand { OrderRequestId = 1, UpdatedBy = 200 };

            var relocation = new Relocation { RelocationId = 100, IsActive = true };
            var relocationReq = new RelocationRequest
            {
                RelocationId = 100,
                OrderRequestId = 1,
                IsActive = true,
            };

            var orderRequest = new OrderRequest
            {
                OrderRequestId = 1,
                SupervisorId = 77,
                ClientId = 2,
                Client = new Client { ClientCode = 567 },
                OrderRequestAssets = new List<Domain.Entities.OrderRequestAssets>(),
            };

            this._orderRequestRepositoryMock.Setup(x => x.GetOrderRequestById(1)).ReturnsAsync(orderRequest);
            this._orderRequestRepositoryMock.Setup(x => x.GetListRelocationRequestByOrderRequestId(1)).ReturnsAsync([
                relocationReq
            ]);
            this._orderRequestRepositoryMock.Setup(x => x.GetRelocationById(100)).ReturnsAsync(relocation);

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            result.Should().Be(Unit.Value);
            this._orderRequestRepositoryMock.Verify(x => x.DeleteRelocationAsync(It.Is<Relocation>(r => r.RelocationId == 100)), Times.Once);
            this._orderRequestRepositoryMock.Verify(x => x.DeleteRelocationRequestAsync(It.Is<RelocationRequest>(r => r.RelocationId == 100)), Times.Once);
        }
    }
}

