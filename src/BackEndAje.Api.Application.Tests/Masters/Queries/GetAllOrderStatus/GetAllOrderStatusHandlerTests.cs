namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllOrderStatus
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllOrderStatus;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllOrderStatusHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllOrderStatusHandler _handler;

        public GetAllOrderStatusHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllOrderStatusHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedOrderStatuses()
        {
            // Arrange
            var userId = 10;

            var orderStatuses = new List<OrderStatus>
            {
                new OrderStatus { OrderStatusId = 1, StatusName = "Generado" },
                new OrderStatus { OrderStatusId = 2, StatusName = "Aprobado" },
            };

            var expected = new List<GetAllOrderStatusResult>
            {
                new GetAllOrderStatusResult { OrderStatusId = 1, StatusName = "Generado" },
                new GetAllOrderStatusResult { OrderStatusId = 2, StatusName = "Aprobado" },
            };

            this._mastersRepositoryMock
                .Setup(repo => repo.GetAllOrderStatus(userId))
                .ReturnsAsync(orderStatuses);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllOrderStatusResult>>(orderStatuses))
                .Returns(expected);

            var query = new GetAllOrderStatusQuery { userId = userId };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}

