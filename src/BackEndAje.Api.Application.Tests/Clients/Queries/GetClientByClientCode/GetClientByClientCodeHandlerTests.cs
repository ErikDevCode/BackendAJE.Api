namespace BackEndAje.Api.Application.Tests.Clients.Queries.GetClientByClientCode
{
    using AutoMapper;
    using BackEndAje.Api.Application.Clients.Queries.GetClientByClientCode;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetClientByClientCodeHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock = new();
        private readonly Mock<IMastersRepository> _mastersRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetClientByClientCodeHandler _handler;

        public GetClientByClientCodeHandlerTests()
        {
            this._handler = new GetClientByClientCodeHandler(this._clientRepoMock.Object, this._mapperMock.Object, this._mastersRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnClient_WhenReasonIsInstallation()
        {
            // Arrange
            var reasons = new List<ReasonRequest>
            {
                new () { ReasonRequestId = 1, ReasonDescription = "Instalación" },
                new () { ReasonRequestId = 2, ReasonDescription = "Retiro" },
                new () { ReasonRequestId = 3, ReasonDescription = "Cambio de Equipo" },
                new () { ReasonRequestId = 4, ReasonDescription = "Servicio Técnico" },
                new () { ReasonRequestId = 5, ReasonDescription = "Reubicación" },
            };

            var request = new GetClientByClientCodeQuery(123, 1, 10, 1);

            var client = new Client { ClientId = 1, ClientCode = 123 };
            var expectedResult = new GetClientByClientCodeResult { ClientId = 1 };

            this._mastersRepoMock.Setup(r => r.GetAllReasonRequest()).ReturnsAsync(reasons);
            this._clientRepoMock.Setup(r => r.GetClientByClientCodeAndRoute(request.ClientCode, request.CediId, request.route)).ReturnsAsync(client);
            this._mapperMock.Setup(m => m.Map<GetClientByClientCodeResult>(client)).Returns(expectedResult);

            // Act
            var result = await this._handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ClientId.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenReasonNotFound()
        {
            // Arrange
            var reasons = new List<ReasonRequest>(); // vacía

            var request = new GetClientByClientCodeQuery(123, 1, 10, 1);

            this._mastersRepoMock.Setup(r => r.GetAllReasonRequest()).ReturnsAsync(reasons);

            // Act
            Func<Task> act = async () => await this._handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No se encontraron todas las razones requeridas en la base de datos.");
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenClientNotFound()
        {
            // Arrange
            var reasons = new List<ReasonRequest>
            {
                new () { ReasonRequestId = 1, ReasonDescription = "Instalación" },
                new () { ReasonRequestId = 2, ReasonDescription = "Retiro" },
                new () { ReasonRequestId = 3, ReasonDescription = "Cambio de Equipo" },
                new () { ReasonRequestId = 4, ReasonDescription = "Servicio Técnico" },
                new () { ReasonRequestId = 5, ReasonDescription = "Reubicación" },
            };

            var request = new GetClientByClientCodeQuery(123, 1, 10, 1);

            this._mastersRepoMock.Setup(r => r.GetAllReasonRequest()).ReturnsAsync(reasons);
            this._clientRepoMock.Setup(r => r.GetClientByClientCodeAndRoute(request.ClientCode, request.CediId, request.route))
                           .ReturnsAsync((Client)null!);

            // Act
            Func<Task> act = async () => await this._handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cliente con código 123 no encontrado en la sucursal.");
        }
    }
}