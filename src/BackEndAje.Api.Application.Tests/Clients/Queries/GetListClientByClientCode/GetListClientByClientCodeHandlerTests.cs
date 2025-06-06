namespace BackEndAje.Api.Application.Tests.Clients.Queries.GetListClientByClientCode
{
    using AutoMapper;
    using BackEndAje.Api.Application.Clients.Queries.GetListClientByClientCode;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetListClientByClientCodeHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetListClientByClientCodeHandler _handler;

        public GetListClientByClientCodeHandlerTests()
        {
            this._handler = new GetListClientByClientCodeHandler(this._clientRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedClientsList()
        {
            // Arrange
            var clientCode = 1234;
            var clients = new List<Client>
            {
                new Client { ClientId = 1, ClientCode = clientCode, CompanyName = "Empresa Uno" },
                new Client { ClientId = 2, ClientCode = clientCode, CompanyName = "Empresa Dos" },
            };

            var mappedResult = new List<GetListClientByClientCodeResult>
            {
                new GetListClientByClientCodeResult { ClientId = 1, ClientCode = clientCode, CompanyName = "Empresa Uno" },
                new GetListClientByClientCodeResult { ClientId = 2, ClientCode = clientCode, CompanyName = "Empresa Dos" },
            };

            var query = new GetListClientByClientCodeQuery(clientCode);

            this._clientRepositoryMock
                .Setup(repo => repo.GetListClientByClientCode(clientCode))!
                .ReturnsAsync(clients);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetListClientByClientCodeResult>>(clients))
                .Returns(mappedResult);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(mappedResult);
        }
    }
}
