namespace BackEndAje.Api.Application.Tests.Assets.Command.UploadAssets
{
    using BackEndAje.Api.Application.Asset.Command.UploadAssets;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;

    public class UploadAssetsHandlerTests
    {
        private readonly Mock<IAssetRepository> _assetRepoMock;
        private readonly UploadAssetsHandler _handler;

        public UploadAssetsHandlerTests()
        {
            this._assetRepoMock = new Mock<IAssetRepository>();
            this._handler = new UploadAssetsHandler(this._assetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddNewAsset_WhenAssetDoesNotExist()
        {
            // Arrange
            var fileBytes = this.CreateExcelWithRow("AJE001", "LogoX", "EEFF", "MarcaX", "ModeloY", "1");

            this._assetRepoMock
                .Setup(repo => repo.GetAssetByCodeAjeAndLogoAndAssetType("AJE001", "LogoX", "EEFF"))
                .ReturnsAsync((Domain.Entities.Asset)null!);

            this._assetRepoMock
                .Setup(repo => repo.AddAssetsAsync(It.IsAny<List<Domain.Entities.Asset>>()))
                .Returns(Task.CompletedTask);

            var command = new UploadAssetsCommand
            {
                FileBytes = fileBytes,
                CreatedBy = 1,
                UpdatedBy = 1,
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.ProcessedClients.Should().Be(1);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldUpdateAsset_WhenAssetExists()
        {
            // Arrange
            var fileBytes = this.CreateExcelWithRow("AJE001", "LogoX", "EEFF", "MarcaX", "ModeloY", "0");

            this._assetRepoMock
                .Setup(repo => repo.GetAssetByCodeAjeAndLogoAndAssetType("AJE001", "LogoX", "EEFF"))
                .ReturnsAsync(new Domain.Entities.Asset { CodeAje = "AJE001" });

            this._assetRepoMock
                .Setup(repo => repo.UpdateAssetAsync(It.IsAny<Domain.Entities.Asset>()))
                .Returns(Task.CompletedTask);

            var command = new UploadAssetsCommand
            {
                FileBytes = fileBytes,
                CreatedBy = 1,
                UpdatedBy = 1,
            };

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.ProcessedClients.Should().Be(1);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldCaptureError_WhenIsActiveIsInvalid()
        {
            // Arrange
            var fileBytes = this.CreateExcelWithRow("AJE002", "LogoY", "EEFF", "MarcaZ", "ModeloZ", "Sí");

            var command = new UploadAssetsCommand
            {
                FileBytes = fileBytes,
                CreatedBy = 1,
                UpdatedBy = 1,
            };

            this._assetRepoMock
                .Setup(repo => repo.GetAssetByCodeAjeAndLogoAndAssetType(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Domain.Entities.Asset)null!);

            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.ProcessedClients.Should().Be(0);
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Message.Should().Contain("Valor inválido para IsActive");
        }

        private byte[] CreateExcelWithRow(string codeAje, string logo, string assetType, string brand, string model, string isActiveText)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Headers
            worksheet.Cells[1, 2].Value = "CodeAje";
            worksheet.Cells[1, 3].Value = "Logo";
            worksheet.Cells[1, 4].Value = "AssetType";
            worksheet.Cells[1, 5].Value = "Brand";
            worksheet.Cells[1, 6].Value = "Model";
            worksheet.Cells[1, 7].Value = "IsActive";

            // Row 2
            worksheet.Cells[2, 2].Value = codeAje;
            worksheet.Cells[2, 3].Value = logo;
            worksheet.Cells[2, 4].Value = assetType;
            worksheet.Cells[2, 5].Value = brand;
            worksheet.Cells[2, 6].Value = model;
            worksheet.Cells[2, 7].Value = isActiveText;

            return package.GetAsByteArray();
        }
    }
}