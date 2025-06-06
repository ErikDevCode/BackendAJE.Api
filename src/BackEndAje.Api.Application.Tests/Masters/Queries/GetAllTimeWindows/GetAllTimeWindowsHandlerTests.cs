namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllTimeWindows
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllTimeWindows;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllTimeWindowsHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllTimeWindowsHandler _handler;

        public GetAllTimeWindowsHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllTimeWindowsHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedTimeWindows()
        {
            // Arrange
            var timeWindowsEntities = new List<TimeWindow>
            {
                new TimeWindow { TimeWindowId = 1, TimeRange = "08:00 - 10:00" },
                new TimeWindow { TimeWindowId = 2, TimeRange = "10:00 - 12:00" },
            };

            var expectedResult = new List<GetAllTimeWindowsResult>
            {
                new GetAllTimeWindowsResult { TimeWindowId = 1, TimeRange = "08:00 - 10:00" },
                new GetAllTimeWindowsResult { TimeWindowId = 2, TimeRange = "10:00 - 12:00" },
            };

            this._mastersRepositoryMock
                .Setup(repo => repo.GetAllTimeWindows())
                .ReturnsAsync(timeWindowsEntities);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllTimeWindowsResult>>(timeWindowsEntities))
                .Returns(expectedResult);

            var query = new GetAllTimeWindowsQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

