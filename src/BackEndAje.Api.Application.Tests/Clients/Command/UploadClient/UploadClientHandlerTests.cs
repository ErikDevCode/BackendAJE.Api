using OfficeOpenXml;

namespace BackEndAje.Api.Application.Tests.Clients.Command.UploadClient
{
    using BackEndAje.Api.Application.Clients.Commands.UploadClient;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class UploadClientHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IMastersRepository> _mastersRepoMock = new();
        private readonly UploadClientsHandler _handler;

        public UploadClientHandlerTests()
        {
            this._handler = new UploadClientsHandler(
                this._clientRepoMock.Object,
                this._mastersRepoMock.Object,
                this._userRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProcessExcelSuccessfully()
        {
            // Arrange
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var documentTypes = new List<DocumentType>
            {
                new () { DocumentTypeId = 1, DocumentTypeName = "DNI" },
            };
            var paymentMethods = new List<PaymentMethods>
            {
                new () { PaymentMethodId = 1, PaymentMethod = "Contado" },
            };
            var users = new List<User>
            {
                new () { UserId = 3, Route = 101 },
            };

            this._mastersRepoMock.Setup(r => r.GetAllDocumentType()).ReturnsAsync(documentTypes);
            this._mastersRepoMock.Setup(r => r.GetAllPaymentMethods()).ReturnsAsync(paymentMethods);
            this._userRepoMock.Setup(r => r.GetUsersByRoutesAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(users);
            this._clientRepoMock.Setup(r => r.GetClientsByClientCodesAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(new List<Client>());
            this._clientRepoMock.Setup(r => r.AddClientsAsync(It.IsAny<List<Client>>())).Returns(Task.CompletedTask);


            byte[] fileBytes;
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Hoja1");
                ws.Cells[1, 1].Value = "Código";
                ws.Cells[1, 2].Value = "Nombre";
                ws.Cells[1, 3].Value = "TipoDocumento";
                ws.Cells[1, 4].Value = "NúmeroDocumento";
                ws.Cells[1, 5].Value = "Correo";
                ws.Cells[1, 6].Value = "FechaEfectiva";
                ws.Cells[1, 7].Value = "MétodoPago";
                ws.Cells[1, 8].Value = "Ruta";
                ws.Cells[1, 9].Value = "Teléfono";
                ws.Cells[1,10].Value = "Dirección";
                ws.Cells[1,11].Value = "Distrito";
                ws.Cells[1,12].Value = "CoordX";
                ws.Cells[1,13].Value = "CoordY";
                ws.Cells[1,14].Value = "Segmentación";

                ws.Cells[2, 1].Value = 123;
                ws.Cells[2, 2].Value = "Ferretería ABC";
                ws.Cells[2, 3].Value = "DNI";
                ws.Cells[2, 4].Value = "12345678";
                ws.Cells[2, 5].Value = "demo@ferreteria.com";
                ws.Cells[2, 6].Value = DateTime.Now.ToShortDateString();
                ws.Cells[2, 7].Value = "Contado";
                ws.Cells[2, 8].Value = 101;
                ws.Cells[2, 9].Value = "999999999";
                ws.Cells[2,10].Value = "Av. Principal";
                ws.Cells[2,11].Value = "Lima";
                ws.Cells[2,12].Value = "-12.0464";
                ws.Cells[2,13].Value = "-77.0428";
                ws.Cells[2,14].Value = "A";

                fileBytes = package.GetAsByteArray();
            }

            var command = new UploadClientsCommand
            {
                FileBytes = fileBytes,
                CreatedBy = 1,
                UpdatedBy = 3,
            };

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.ProcessedClients.Should().Be(1);
        }
    }
}

