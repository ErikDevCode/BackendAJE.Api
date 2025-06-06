namespace BackEndAje.Api.Application.Tests.OrderRequests.Queries.GetTrackingByOrderRequestId
{
    using AutoMapper;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingByOrderRequestId;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetTrackingByOrderRequestIdHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTrackingByOrderRequestIdHandler _handler;

        public GetTrackingByOrderRequestIdHandlerTests()
        {
            this._orderRequestRepositoryMock = new Mock<IOrderRequestRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetTrackingByOrderRequestIdHandler(
                this._orderRequestRepositoryMock.Object,
                this._mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Results_When_History_Exists()
        {
            // Arrange
            var orderRequestId = 101;
            var query = new GetTrackingByOrderRequestIdQuery(orderRequestId);

            var historyEntities = new List<OrderRequestStatusHistory>
            {
                new OrderRequestStatusHistory { OrderStatusHistoryId = 1 },
                new OrderRequestStatusHistory { OrderStatusHistoryId = 2 },
            };

            var mappedResults = new List<GetTrackingByOrderRequestIdResult>
            {
                new GetTrackingByOrderRequestIdResult { OrderRequestId = 1 },
                new GetTrackingByOrderRequestIdResult { OrderRequestId = 2 },
            };

            this._orderRequestRepositoryMock
                .Setup(repo => repo.GetOrderRequestStatusHistoryByOrderRequestId(orderRequestId))
                .ReturnsAsync(historyEntities);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetTrackingByOrderRequestIdResult>>(historyEntities))
                .Returns(mappedResults);

            // Act
            var result = await this._handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.Select(x => x.OrderRequestId).Should().Contain(1).And.Contain(2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)] // vacía
        public async Task Handle_Should_Throw_When_History_Not_Found(int? caseType)
        {
            // Arrange
            var orderRequestId = 202;
            var query = new GetTrackingByOrderRequestIdQuery(orderRequestId);

            this._orderRequestRepositoryMock
                .Setup(repo => repo.GetOrderRequestStatusHistoryByOrderRequestId(orderRequestId))!
                .ReturnsAsync(caseType is null ? null : new List<OrderRequestStatusHistory>());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                this._handler.Handle(query, CancellationToken.None));
        }
    }
}

