namespace BackEndAje.Api.Application.Tests.Clients.Command.UploadCodeAndNameClients
{
    using BackEndAje.Api.Application.Clients.Commands.UploadCodeAndNameClients;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;

    public class UploadCodeAndNameClientsHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock = new();
        private readonly UploadCodeAndNameClientsHandler _handler;

        public UploadCodeAndNameClientsHandlerTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            this._handler = new UploadCodeAndNameClientsHandler(this._clientRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateClientsSuccessfully()
        {
            // Arrange
            var existingClients = new List<Client>
            {
                new Client { ClientId = 1, ClientCode = 111, CompanyName = "Old Name 1" },
                new Client { ClientId = 2, ClientCode = 222, CompanyName = "Old Name 2" },
            };

            this._clientRepoMock.Setup(r => r.GetClientsOnlyList()).ReturnsAsync(existingClients);
            this._clientRepoMock.Setup(r => r.UpdateClientsAsync(It.IsAny<List<Client>>()))
                .Returns(Task.CompletedTask);

            // Crear archivo Excel válido
            byte[] fileBytes;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "ClientId";
                worksheet.Cells[1, 2].Value = "ClientCode";
                worksheet.Cells[1, 3].Value = "CompanyName";

                worksheet.Cells[2, 1].Value = 1;
                worksheet.Cells[2, 2].Value = 999;
                worksheet.Cells[2, 3].Value = "Nuevo Nombre 1";

                worksheet.Cells[3, 1].Value = 2;
                worksheet.Cells[3, 2].Value = 888;
                worksheet.Cells[3, 3].Value = "Nuevo Nombre 2";

                fileBytes = package.GetAsByteArray();
            }

            var command = new UploadCodeAndNameClientsCommand
            {
                FileBytes = fileBytes,
                UpdatedBy = 123,
            };

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.UpdatedCount.Should().Be(2);
        }
    }
}

