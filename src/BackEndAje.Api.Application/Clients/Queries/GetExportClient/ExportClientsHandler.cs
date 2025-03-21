namespace BackEndAje.Api.Application.Clients.Queries.GetExportClient
{
    using System.Drawing;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class ExportClientsHandler : IRequestHandler<ExportClientsQuery, byte[]>
    {
        private readonly IClientRepository _clientRepository;

        public ExportClientsHandler(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }

        public async Task<byte[]> Handle(ExportClientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var clients = await this._clientRepository.GetClientsList();

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Solicitud");

                worksheet.Cells[1, 1, 1, 15].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 15].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D3D3D3"));
                worksheet.Cells[1, 1, 1, 15].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Codigo de Cliente";
                worksheet.Cells[1, 3].Value = "Nombre de Compañia";
                worksheet.Cells[1, 4].Value = "Tipo de Documento";
                worksheet.Cells[1, 5].Value = "Numero de Documento";
                worksheet.Cells[1, 6].Value = "Email";

                worksheet.Cells[1, 7].Value = "Fecha Efectiva";
                worksheet.Cells[1, 8].Value = "Metodo de pago";
                worksheet.Cells[1, 9].Value = "Route";
                worksheet.Cells[1, 10].Value = "Celular";
                worksheet.Cells[1, 11].Value = "Direccion";
                worksheet.Cells[1, 12].Value = "Distrito";
                worksheet.Cells[1, 13].Value = "CoordX";
                worksheet.Cells[1, 14].Value = "CoordY";
                worksheet.Cells[1, 15].Value = "Segmentacion";
                worksheet.Cells[1, 16].Value = "Activo";

                var row = 2;
                foreach (var client in clients)
                {
                    worksheet.Cells[row, 1].Value = client.ClientId;
                    worksheet.Cells[row, 2].Value = client.ClientCode;
                    worksheet.Cells[row, 3].Value = client.CompanyName ?? "";
                    worksheet.Cells[row, 4].Value = client.DocumentType?.DocumentTypeName ?? "";
                    worksheet.Cells[row, 5].Value = client.DocumentNumber ?? "";
                    worksheet.Cells[row, 6].Value = client.Email ?? "";
                    worksheet.Cells[row, 7].Value = client.EffectiveDate?.ToString() ?? "";
                    worksheet.Cells[row, 8].Value = client.PaymentMethod?.PaymentMethod ?? "";
                    worksheet.Cells[row, 9].Value = client.Route;
                    worksheet.Cells[row, 10].Value = client.Phone ?? "";
                    worksheet.Cells[row, 11].Value = client.Address ?? "";
                    worksheet.Cells[row, 12].Value = client.District?.Name ?? "";
                    worksheet.Cells[row, 13].Value = client.CoordX?.ToString() ?? "";
                    worksheet.Cells[row, 14].Value = client.CoordY?.ToString() ?? "";
                    worksheet.Cells[row, 15].Value = client.Segmentation ?? "";
                    worksheet.Cells[row, 16].Value = client.IsActive ? "HABILITADO" : "DESHABILITADO";
                    row++;
                }

                var dataRange = worksheet.Cells[1, 1, row - 1, 16];
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

