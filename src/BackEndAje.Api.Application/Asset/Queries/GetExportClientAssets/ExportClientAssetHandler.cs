namespace BackEndAje.Api.Application.Asset.Queries.GetExportClientAssets
{
    using System.Drawing;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class ExportClientAssetHandler : IRequestHandler<ExportClientAssetQuery, byte[]>
    {
        private readonly IClientAssetRepository _clientAssetRepository;

        public ExportClientAssetHandler(IClientAssetRepository clientAssetRepository)
        {
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<byte[]> Handle(ExportClientAssetQuery request, CancellationToken cancellationToken)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var clientsAssets = await this._clientAssetRepository.GetClientAssetsListAsync();

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("ClientesConActivos");

                worksheet.Cells[1, 1, 1, 9].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 9].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D3D3D3"));
                worksheet.Cells[1, 1, 1, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                worksheet.Cells[1, 1].Value = "Fecha Instalación";
                worksheet.Cells[1, 2].Value = "Sucursal";
                worksheet.Cells[1, 3].Value = "Código Cliente";
                worksheet.Cells[1, 4].Value = "Nombre de Compañia";
                worksheet.Cells[1, 5].Value = "Ruta";

                worksheet.Cells[1, 6].Value = "Codigo AJE";
                worksheet.Cells[1, 7].Value = "Brand";
                worksheet.Cells[1, 8].Value = "Observación";
                worksheet.Cells[1, 9].Value = "Estado";
                worksheet.Cells[1, 10].Value = "Fecha Creación";

                var row = 2;
                foreach (var client in clientsAssets)
                {
                    worksheet.Cells[row, 1].Value = client.InstallationDate?.ToString("dd/MM/yyyy") ?? string.Empty;
                    worksheet.Cells[row, 2].Value = client.CediName;
                    worksheet.Cells[row, 3].Value = client.ClientCode;
                    worksheet.Cells[row, 4].Value = client.ClientName;
                    worksheet.Cells[row, 5].Value = client.Route;
                    worksheet.Cells[row, 6].Value = client.CodeAje;
                    worksheet.Cells[row, 7].Value = client.Brand;
                    worksheet.Cells[row, 8].Value = client.Notes;
                    worksheet.Cells[row, 9].Value = (bool)client.IsActive! ? "HABILITADO" : "DESHABILITADO";
                    worksheet.Cells[row, 10].Value = client.CreatedAt.ToString("dd/MM/yyyy");
                    row++;
                }

                var dataRange = worksheet.Cells[1, 1, row - 1, 10];
                dataRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                dataRange.Style.Border.Top.Color.SetColor(Color.Black);
                dataRange.Style.Border.Left.Color.SetColor(Color.Black);
                dataRange.Style.Border.Right.Color.SetColor(Color.Black);
                dataRange.Style.Border.Bottom.Color.SetColor(Color.Black);

                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

