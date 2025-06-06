namespace BackEndAje.Api.Application.Tests.Census.Queries.GetCensusQuestions
{
    using AutoFixture;
    using AutoMapper;
    using BackEndAje.Api.Application.Census.Queries.GetCensusQuestions;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetCensusQuestionsHandlerTests
    {
        private readonly Mock<ICensusRepository> _censusRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCensusQuestionsHandler _handler;
        private readonly Fixture _fixture;

        public GetCensusQuestionsHandlerTests()
        {
            this._censusRepoMock = new Mock<ICensusRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetCensusQuestionsHandler(this._censusRepoMock.Object, this._mapperMock.Object);
            this._fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedQuestions_WhenQuestionsExist()
        {
            // Arrange
            var clientId = 123;
            var request = new GetCensusQuestionsQuery(clientId);

            var censusQuestions = this._fixture.CreateMany<CensusQuestion>(5).ToList();
            var expectedResult = this._fixture.CreateMany<GetCensusQuestionsResult>(5).ToList();

            this._censusRepoMock
                .Setup(repo => repo.GetCensusQuestions(clientId))
                .ReturnsAsync(censusQuestions);

            this._mapperMock
                .Setup(m => m.Map<List<GetCensusQuestionsResult>>(censusQuestions))
                .Returns(expectedResult);

            // Act
            var result = await this._handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            this._censusRepoMock.Verify(r => r.GetCensusQuestions(clientId), Times.Once);
            this._mapperMock.Verify(m => m.Map<List<GetCensusQuestionsResult>>(censusQuestions), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenQuestionsAreEmpty()
        {
            // Arrange
            var clientId = 123;
            var request = new GetCensusQuestionsQuery(clientId);

            this._censusRepoMock
                .Setup(repo => repo.GetCensusQuestions(clientId))
                .ReturnsAsync([]);

            // Act
            Func<Task> act = async () => await this._handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("El cliente ya fue censado para el período actual.");

            this._censusRepoMock.Verify(r => r.GetCensusQuestions(clientId), Times.Once);
            this._mapperMock.Verify(m => m.Map<List<GetCensusQuestionsResult>>(It.IsAny<List<CensusQuestion>>()), Times.Never);
        }
    }
}