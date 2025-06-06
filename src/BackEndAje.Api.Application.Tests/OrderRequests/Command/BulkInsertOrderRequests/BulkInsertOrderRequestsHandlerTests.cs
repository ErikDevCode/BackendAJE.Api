namespace BackEndAje.Api.Application.Tests.OrderRequests.Command.BulkInsertOrderRequests
{
    using AutoFixture;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.OrderRequests.Commands.BulkInsertOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;
    using OfficeOpenXml;

    public class BulkInsertOrderRequestsHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<ICediRepository> _cediRepositoryMock = new();
        private readonly Mock<IMastersRepository> _mastersRepositoryMock = new();
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IAssetRepository> _assetRepositoryMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();

        private readonly BulkInsertOrderRequestsHandler _handler;

        public BulkInsertOrderRequestsHandlerTests()
        {
            this._handler = new BulkInsertOrderRequestsHandler(
                this._orderRequestRepositoryMock.Object,
                this._userRepositoryMock.Object,
                this._cediRepositoryMock.Object,
                this._mastersRepositoryMock.Object,
                this._clientRepositoryMock.Object,
                this._mediatorMock.Object,
                this._assetRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Process_Valid_ExcelFile()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var command = fixture.Build<BulkInsertOrderRequestsCommand>()
                .With(x => x.ReasonRequest, (int)ReasonRequestConst.Retiro)
                .With(x => x.File, this.CreateSampleExcelFile())
                .Create();

            var user = fixture.Create<User>();
            var cedi = fixture.Create<Cedi>();
            var reason = fixture.Create<ReasonRequest>();
            var client = fixture.Create<Client>();
            var timeWindow = fixture.Create<TimeWindow>();
            var productSize = fixture.Create<ProductSize>();
            var asset = new List<Domain.Entities.Asset> { fixture.Create<Domain.Entities.Asset>() };
            var withDrawalReason = fixture.Create<WithDrawalReason>();

            this._userRepositoryMock.Setup(x => x.GetUserByDocumentNumberAsync(It.IsAny<string>())).ReturnsAsync(user);
            this._cediRepositoryMock.Setup(x => x.GetCediByNameAsync(It.IsAny<string>())).ReturnsAsync(cedi);
            this._mastersRepositoryMock.Setup(x => x.GetReasonRequestByDescriptionAsync(It.IsAny<string>())).ReturnsAsync(reason);
            this._clientRepositoryMock.Setup(x => x.GetClientByClientCode(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(client);
            this._orderRequestRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(false);
            this._mastersRepositoryMock.Setup(x => x.GetTimeWindowsByTimeRangeAsync(It.IsAny<string>())).ReturnsAsync(timeWindow);
            this._mastersRepositoryMock.Setup(x => x.GetProductSizeByDescriptionAsync(It.IsAny<string>())).ReturnsAsync(productSize);
            this._mastersRepositoryMock.Setup(x => x.GetWithDrawalReasonsByDescriptionAsync(It.IsAny<string>())).ReturnsAsync(withDrawalReason);
            this._assetRepositoryMock.Setup(x => x.GetAssetByCodeAje(It.IsAny<string>())).ReturnsAsync(asset);
            this._mediatorMock.Setup(x => x.Send(It.IsAny<CreateOrderRequestsCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
        }

        private byte[] CreateSampleExcelFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells[1, 1].Value = "DNI";
            worksheet.Cells[1, 2].Value = "Sucursal";
            worksheet.Cells[1, 3].Value = "TipoSolicitud";
            worksheet.Cells[1, 4].Value = "FechaNegociada";
            worksheet.Cells[1, 5].Value = "VentanaTiempo";
            worksheet.Cells[1, 6].Value = "CodigoCliente";
            worksheet.Cells[1, 7].Value = "Observaciones";
            worksheet.Cells[1, 8].Value = "Referencia";
            worksheet.Cells[1, 9].Value = "TamañoProducto";
            worksheet.Cells[1, 10].Value = "CodeAje";
            worksheet.Cells[1, 11].Value = "MotivoRetiro";

            worksheet.Cells[2, 1].Value = "12345678";
            worksheet.Cells[2, 2].Value = "Sucursal X";
            worksheet.Cells[2, 3].Value = "Retiro";
            worksheet.Cells[2, 4].Value = DateTime.Now.ToShortDateString();
            worksheet.Cells[2, 5].Value = "8:00 - 10:00";
            worksheet.Cells[2, 6].Value = "1001";
            worksheet.Cells[2, 7].Value = "Observación test";
            worksheet.Cells[2, 8].Value = "Referencia test";
            worksheet.Cells[2, 9].Value = "Grande";
            worksheet.Cells[2, 10].Value = "AJE-001";
            worksheet.Cells[2, 11].Value = "Mal estado";

            return package.GetAsByteArray();
        }

        [Fact]
        public async Task Handle_Should_Not_CreateOrder_If_OrderAlreadyExists()
        {
            // Arrange
            var command = new BulkInsertOrderRequestsCommand
            {
                File = this.CreateSampleExcelFile(),
                ReasonRequest = (int)ReasonRequestConst.Retiro,
            };

            var user = new User { UserId = 1, DocumentNumber = "12345678", Names = "Test" };

            this._userRepositoryMock
                .Setup(x => x.GetUserByDocumentNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            this._cediRepositoryMock
                .Setup(x => x.GetCediByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new Cedi());

            this._mastersRepositoryMock
                .Setup(x => x.GetReasonRequestByDescriptionAsync(It.IsAny<string>()))
                .ReturnsAsync(new ReasonRequest());

            this._clientRepositoryMock
                .Setup(x => x.GetClientByClientCode(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Client());

            this._orderRequestRepositoryMock
                .Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            this._mediatorMock.Verify(
                x => x.Send(It.IsAny<CreateOrderRequestsCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}

