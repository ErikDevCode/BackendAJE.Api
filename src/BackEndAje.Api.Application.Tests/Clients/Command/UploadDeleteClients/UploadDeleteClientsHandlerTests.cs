namespace BackEndAje.Api.Application.Tests.Clients.Command.UploadDeleteClients
{
    using BackEndAje.Api.Application.Clients.Commands.UploadDeleteClients;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;

    public class UploadDeleteClientsHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepoMock = new();
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock = new();
        private readonly UploadDeleteClientsHandler _handler;

        public UploadDeleteClientsHandlerTests()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this._handler = new UploadDeleteClientsHandler(
                this._clientRepoMock.Object,
                this._clientAssetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeactivateClientsSuccessfully()
        {
            // Arrange
            var clientsInDb = new List<Client>
            {
                new Client { ClientId = 1, ClientCode = 1001, CompanyName = "Ferretería Uno", IsActive = true },
                new Client { ClientId = 2, ClientCode = 1002, CompanyName = "Ferretería Dos", IsActive = true },
            };

            var clientAssets = new List<ClientAssets>
            {
                new ClientAssets { ClientId = 1 },
                new ClientAssets { ClientId = 2 },
            };

            this._clientRepoMock.Setup(r => r.GetClientsOnlyList()).ReturnsAsync(clientsInDb);
            this._clientRepoMock.Setup(r => r.UpdateClientsAsync(It.IsAny<List<Client>>())).Returns(Task.CompletedTask);

            this._clientAssetRepoMock.Setup(r => r.GetClientAssetsByClientId(It.IsAny<int>())).ReturnsAsync((int id) => clientAssets.Where(a => a.ClientId == id).ToList());
            this._clientAssetRepoMock.Setup(r => r.DeleteRangeAsync(It.IsAny<List<ClientAssets>>())).Returns(Task.CompletedTask);

            byte[] fileBytes;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "ClientCode";
                worksheet.Cells[1, 2].Value = "CompanyName";

                worksheet.Cells[2, 1].Value = 1001;
                worksheet.Cells[2, 2].Value = "Ferretería Uno";
                worksheet.Cells[3, 1].Value = 1002;
                worksheet.Cells[3, 2].Value = "Ferretería Dos";

                fileBytes = package.GetAsByteArray();
            }

            var command = new UploadDeleteClientsCommand
            {
                FileBytes = fileBytes,
                UpdatedBy = 10,
            };

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.UpdatedCount.Should().Be(2);

            clientsInDb.All(c => c.IsActive == false).Should().BeTrue();
        }
    }
}
