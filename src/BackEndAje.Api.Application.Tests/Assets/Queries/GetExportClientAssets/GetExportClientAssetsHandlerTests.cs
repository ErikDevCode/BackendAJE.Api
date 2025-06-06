namespace BackEndAje.Api.Application.Tests.Assets.Queries.GetExportClientAssets
{
    using AutoFixture;
    using BackEndAje.Api.Application.Asset.Queries.GetExportClientAssets;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;

    public class GetExportClientAssetsHandlerTests
    {
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock;
        private readonly ExportClientAssetHandler _handler;

        public GetExportClientAssetsHandlerTests()
        {
            this._clientAssetRepoMock = new Mock<IClientAssetRepository>();
            this._handler = new ExportClientAssetHandler(this._clientAssetRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldGenerateExcelFile_WhenDataExists()
        {
            // Arrange
            var fixture = new Fixture();
            var data = fixture.Build<ClientAssetsDto>()
                .With(c => c.ClientCode, 123)
                .With(c => c.ClientName, "Ferretería ABC")
                .With(c => c.IsActive, true)
                .With(c => c.CreatedAt, new DateTime(2024, 1, 2))
                .CreateMany(5)
                .ToList();


            this._clientAssetRepoMock
                .Setup(r => r.GetClientAssetsListAsync())
                .ReturnsAsync(data);

            var query = new ExportClientAssetQuery();

            // Act
            var resultBytes = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultBytes.Should().NotBeNullOrEmpty();

            // Validar contenido básico del Excel generado
            using var package = new ExcelPackage(new MemoryStream(resultBytes));
            var worksheet = package.Workbook.Worksheets["ClientesConActivos"];
            worksheet.Should().NotBeNull("La hoja 'ClientesConActivos' debe existir");

            // Validar encabezados
            worksheet.Cells[1, 1].Text.Should().Be("Fecha Instalación");
            worksheet.Cells[1, 6].Text.Should().Be("Codigo AJE");
            worksheet.Cells[1, 9].Text.Should().Be("Estado");

            // Validar contenido de la primera fila de datos
            worksheet.Cells[2, 3].Text.Should().Be("123");
            worksheet.Cells[2, 4].Text.Should().Be("Ferretería ABC");
            worksheet.Cells[2, 9].Text.Should().Be("HABILITADO");
        }
    }
}

