namespace BackEndAje.Api.Application.Tests.OrderRequests.Queries.GetOrderRequestById
{
    using AutoMapper;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetOrderRequestByIdHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOrderRequestByIdHandler _handler;

        public GetOrderRequestByIdHandlerTests()
        {
            this._orderRequestRepositoryMock = new Mock<IOrderRequestRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetOrderRequestByIdHandler(
                this._orderRequestRepositoryMock.Object,
                this._mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Result_When_OrderRequest_Exists()
        {
            // Arrange
            var orderRequestId = 123;
            var request = new GetOrderRequestByIdQuery(orderRequestId);

            var orderRequestEntity = new OrderRequest { OrderRequestId = orderRequestId };
            var expectedResult = new GetOrderRequestByIdResult { OrderRequestId = orderRequestId };

            this._orderRequestRepositoryMock
                .Setup(repo => repo.GetOrderRequestById(orderRequestId))
                .ReturnsAsync(orderRequestEntity);

            this._mapperMock
                .Setup(mapper => mapper.Map<GetOrderRequestByIdResult>(orderRequestEntity))
                .Returns(expectedResult);

            // Act
            var result = await this._handler.Handle(request, default);

            // Assert
            result.Should().NotBeNull();
            result.OrderRequestId.Should().Be(orderRequestId);
        }

        [Fact]
        public async Task Handle_Should_Throw_KeyNotFoundException_When_OrderRequest_Does_Not_Exist()
        {
            // Arrange
            var request = new GetOrderRequestByIdQuery(999);

            this._orderRequestRepositoryMock
                .Setup(repo => repo.GetOrderRequestById(request.orderRequestId))
                .ReturnsAsync((OrderRequest?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                this._handler.Handle(request, CancellationToken.None));
        }
    }
}