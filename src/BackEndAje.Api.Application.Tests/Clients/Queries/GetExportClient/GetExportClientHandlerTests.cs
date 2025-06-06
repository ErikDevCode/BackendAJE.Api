namespace BackEndAje.Api.Application.Tests.Clients.Queries.GetExportClient
{
    using BackEndAje.Api.Application.Clients.Queries.GetExportClient;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;

    public class GetExportClientHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock = new();
        private readonly ExportClientsHandler _handler;

        public GetExportClientHandlerTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this._handler = new ExportClientsHandler(this._clientRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnExcelFileWithClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client
                {
                    ClientId = 1,
                    ClientCode = 1001,
                    CompanyName = "Empresa A",
                    DocumentType = new DocumentType { DocumentTypeName = "DNI" },
                    DocumentNumber = "12345678",
                    Email = "cliente@empresa.com",
                    EffectiveDate = new System.DateTime(2024, 1, 1),
                    PaymentMethod = new PaymentMethods { PaymentMethod = "Contado" },
                    Route = 12,
                    Phone = "999999999",
                    Address = "Calle Falsa 123",
                    District = new District { Name = "Lima" },
                    CoordX = "22.2",
                    CoordY = "24.3",
                    Segmentation = "A",
                    IsActive = true,
                },
            };

            this._clientRepoMock.Setup(r => r.GetClientsList()).ReturnsAsync(clients);

            var query = new ExportClientsQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);

            using var package = new ExcelPackage(new MemoryStream(result));
            var worksheet = package.Workbook.Worksheets["Solicitud"];
            worksheet.Should().NotBeNull();
            worksheet.Cells[2, 3].Text.Should().Be("Empresa A");
            worksheet.Cells[2, 4].Text.Should().Be("DNI");
        }
    }
}
