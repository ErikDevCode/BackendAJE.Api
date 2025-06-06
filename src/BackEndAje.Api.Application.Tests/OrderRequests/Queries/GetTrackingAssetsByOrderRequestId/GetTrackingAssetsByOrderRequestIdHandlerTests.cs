namespace BackEndAje.Api.Application.Tests.OrderRequests.Queries.GetTrackingAssetsByOrderRequestId
{
    using AutoMapper;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingAssetsByOrderRequestId;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetTrackingAssetsByOrderRequestIdHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTrackingAssetsByOrderRequestIdHandler _handler;

        public GetTrackingAssetsByOrderRequestIdHandlerTests()
        {
            this._orderRequestRepositoryMock = new Mock<IOrderRequestRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetTrackingAssetsByOrderRequestIdHandler(
                this._orderRequestRepositoryMock.Object,
                this._mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Results_When_TrackingAssets_Exist()
        {
            // Arrange
            var orderRequestId = 42;
            var query = new GetTrackingAssetsByOrderRequestIdQuery(orderRequestId);

            var entities = new List<OrderRequestAssetsTrace>
            {
                new OrderRequestAssetsTrace { OrderRequestAssetTraceId = 1 },
                new OrderRequestAssetsTrace { OrderRequestAssetTraceId = 2 },
            };

            var mappedResults = new List<GetTrackingAssetsByOrderRequestIdResult>
            {
                new GetTrackingAssetsByOrderRequestIdResult { OrderRequestId = 1 },
                new GetTrackingAssetsByOrderRequestIdResult { OrderRequestId = 2 },
            };

            this._orderRequestRepositoryMock
                .Setup(repo => repo.GetOrderRequestAssetsTraceByOrderRequestId(orderRequestId))
                .ReturnsAsync(entities);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetTrackingAssetsByOrderRequestIdResult>>(entities))
                .Returns(mappedResults);

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.Select(r => r.OrderRequestId).Should().Contain(1).And.Contain(2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public async Task Handle_Should_Throw_When_TrackingAssets_NotFound(int? caseType)
        {
            // Arrange
            var orderRequestId = 77;
            var query = new GetTrackingAssetsByOrderRequestIdQuery(orderRequestId);

            this._orderRequestRepositoryMock
                .Setup(repo => repo.GetOrderRequestAssetsTraceByOrderRequestId(orderRequestId))!
                .ReturnsAsync(caseType is null ? null : new List<OrderRequestAssetsTrace>());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                this._handler.Handle(query, default));
        }
    }
}

