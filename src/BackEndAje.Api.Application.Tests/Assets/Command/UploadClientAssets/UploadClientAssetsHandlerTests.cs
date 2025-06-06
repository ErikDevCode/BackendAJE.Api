namespace BackEndAje.Api.Application.Tests.Assets.Command.UploadClientAssets
{
    using BackEndAje.Api.Application.Asset.Command.UploadClientAssets;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;

    public class UploadClientAssetsHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepoMock = new();
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock = new();
        private readonly Mock<ICediRepository> _cediRepoMock = new();
        private readonly Mock<IClientRepository> _clientRepoMock = new();
        private readonly UploadClientAssetsHandler _handler;

        public UploadClientAssetsHandlerTests()
        {
            this._handler = new UploadClientAssetsHandler(
                this._assetRepoMock.Object,
                this._clientAssetRepoMock.Object,
                this._cediRepoMock.Object,
                this._clientRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateClientAsset_WhenValid()
        {
            // Arrange
            var cedi = new Cedi { CediId = 1, CediName = "Sucursal A" };
            var client = new Client
            {
                ClientId = 101,
                ClientCode = 5001,
                Seller = new User { CediId = 1 },
            };
            var asset = new Domain.Entities.Asset { AssetId = 999, CodeAje = "AJE001" };

            var fileBytes = this.CreateExcelWithClientAsset(
                cedi.CediName, "01/01/2024", client.ClientCode.ToString(), asset.CodeAje, "Nota X");

            this._cediRepoMock.Setup(r => r.GetAllCedis()).ReturnsAsync(new List<Cedi> { cedi });
            this._clientRepoMock.Setup(r => r.GetClientsList()).ReturnsAsync(new List<Client> { client });
            this._assetRepoMock.Setup(r => r.GetAssetsList()).ReturnsAsync(new List<Domain.Entities.Asset> { asset });
            this._clientAssetRepoMock.Setup(r => r.GetClientAssetByAssetId(asset.AssetId))
                                .ReturnsAsync(new List<ClientAssets>());

            this._clientAssetRepoMock.Setup(r => r.AddClientListAsset(It.IsAny<List<ClientAssets>>()))
                                .Returns(Task.CompletedTask);
            this._clientAssetRepoMock.Setup(r => r.AddTraceabilityRecordListAsync(It.IsAny<List<ClientAssetsTrace>>()))
                                .Returns(Task.CompletedTask);

            var command = new UploadClientAssetsCommand
            {
                FileBytes = fileBytes,
                CreatedBy = 10,
                UpdatedBy = 20,
            };

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.ProcessedAssets.Should().Be(1);
            result.Errors.Should().BeEmpty();

            this._clientAssetRepoMock.Verify(
                r => r.AddClientListAsset(It.Is<List<ClientAssets>>(list =>
                list.Count == 1 &&
                list[0].ClientId == client.ClientId &&
                list[0].AssetId == asset.AssetId &&
                list[0].CodeAje == asset.CodeAje &&
                list[0].IsActive == true
            )), Times.Once);

            this._clientAssetRepoMock.Verify(r => r.AddTraceabilityRecordListAsync(It.IsAny<List<ClientAssetsTrace>>()), Times.Once);
        }

        private byte[] CreateExcelWithClientAsset(string cedi, string date, string clientCode, string codeAje, string notes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Activos");

            // Headers
            sheet.Cells[1, 1].Value = "Cedi";
            sheet.Cells[1, 2].Value = "Fecha Instalación";
            sheet.Cells[1, 3].Value = "Código Cliente";
            sheet.Cells[1, 4].Value = "CodeAje";
            sheet.Cells[1, 5].Value = "Notas";

            // Row 2
            sheet.Cells[2, 1].Value = cedi;
            sheet.Cells[2, 2].Value = date;
            sheet.Cells[2, 3].Value = clientCode;
            sheet.Cells[2, 4].Value = codeAje;
            sheet.Cells[2, 5].Value = notes;

            return package.GetAsByteArray();
        }
    }
}
