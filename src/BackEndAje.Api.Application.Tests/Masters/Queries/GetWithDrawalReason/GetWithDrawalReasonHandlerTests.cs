namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetWithDrawalReason
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetWithDrawalReasonHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetWithDrawalReasonHandler _handler;

        public GetWithDrawalReasonHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetWithDrawalReasonHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedWithDrawalReasons()
        {
            // Arrange
            var reasonRequestId = 2;

            var withdrawalReasonEntities = new List<WithDrawalReason>
            {
                new WithDrawalReason { WithDrawalReasonId = 1, WithDrawalReasonDescription = "Producto defectuoso" },
                new WithDrawalReason { WithDrawalReasonId = 2, WithDrawalReasonDescription = "Cambio de producto" },
            };

            var expectedResult = new List<GetWithDrawalReasonResult>
            {
                new GetWithDrawalReasonResult { WithDrawalReasonId = 1, WithDrawalReasonDescription = "Producto defectuoso" },
                new GetWithDrawalReasonResult { WithDrawalReasonId = 2, WithDrawalReasonDescription = "Cambio de producto" },
            };

            this._mastersRepositoryMock
                .Setup(repo => repo.GetWithDrawalReasonsByReasonRequestId(reasonRequestId))
                .ReturnsAsync(withdrawalReasonEntities);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetWithDrawalReasonResult>>(withdrawalReasonEntities))
                .Returns(expectedResult);

            var query = new GetWithDrawalReasonQuery(reasonRequestId);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

