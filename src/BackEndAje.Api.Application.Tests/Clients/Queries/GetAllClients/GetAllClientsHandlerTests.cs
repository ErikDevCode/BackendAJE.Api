namespace BackEndAje.Api.Application.Tests.Clients.Queries.GetAllClients
{
    using AutoMapper;
    using BackEndAje.Api.Application.Clients.Queries.GetAllClients;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllClientsHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<ICensusRepository> _censusRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetAllClientsHandler _handler;

        public GetAllClientsHandlerTests()
        {
            this._handler = new GetAllClientsHandler(
                this._clientRepositoryMock.Object,
                this._mapperMock.Object,
                this._censusRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedClientsWithCensusFlag()
        {
            // Arrange
            var query = new GetAllClientsQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Filtro = null,
                UserId = 1,
            };

            var clients = new List<Client>
            {
                new Client { ClientId = 1, ClientAssets = [] },
                new Client { ClientId = 2, ClientAssets = [] },
            };

            var mappedResults = new List<GetAllClientsResult>
            {
                new GetAllClientsResult { ClientId = 1 },
                new GetAllClientsResult { ClientId = 2 },
            };

            var censusClientIds = new List<int> { 1 };

            this._clientRepositoryMock.Setup(r => r.GetClients(query.PageNumber, query.PageSize, query.Filtro, query.UserId))
                .ReturnsAsync(clients);

            this._mapperMock.Setup(m => m.Map<List<GetAllClientsResult>>(clients))
                .Returns(mappedResults);

            this._censusRepositoryMock.Setup(r => r.GetCensusClientIdsByPeriodAsync(It.IsAny<string>(), It.IsAny<List<int>>()))
                .ReturnsAsync(censusClientIds);

            this._clientRepositoryMock.Setup(r => r.GetTotalClients(query.Filtro, query.UserId))
                .ReturnsAsync(2);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(2);
            result.Items.Should().HaveCount(2);
            result.Items.First(i => i.ClientId == 1).IsCensus.Should().BeTrue();
            result.Items.First(i => i.ClientId == 2).IsCensus.Should().BeFalse();
        }
    }
}
