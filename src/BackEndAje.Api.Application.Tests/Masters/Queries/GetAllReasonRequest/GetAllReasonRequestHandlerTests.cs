namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllReasonRequest
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllReasonRequestHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllReasonRequestHandler _handler;

        public GetAllReasonRequestHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllReasonRequestHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedReasonRequests()
        {
            // Arrange
            var reasonEntities = new List<ReasonRequest>
            {
                new ReasonRequest { ReasonRequestId = 1, ReasonDescription = "Instalación" },
                new ReasonRequest { ReasonRequestId = 2, ReasonDescription = "Retiro" },
            };

            var expectedResult = new List<GetAllReasonRequestResult>
            {
                new GetAllReasonRequestResult { ReasonRequestId = 1, ReasonDescription = "Instalación" },
                new GetAllReasonRequestResult { ReasonRequestId = 2, ReasonDescription = "Retiro" },
            };

            this._mastersRepositoryMock
                .Setup(repo => repo.GetAllReasonRequest())
                .ReturnsAsync(reasonEntities);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllReasonRequestResult>>(reasonEntities))
                .Returns(expectedResult);

            var query = new GetAllReasonRequestQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

