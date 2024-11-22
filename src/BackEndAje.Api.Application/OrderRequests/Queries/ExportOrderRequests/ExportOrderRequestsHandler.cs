namespace BackEndAje.Api.Application.OrderRequests.Queries.ExportOrderRequests
{
    using System.Drawing;
    using System.Globalization;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class ExportOrderRequestsHandler : IRequestHandler<ExportOrderRequestsQuery, byte[]>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IUserRepository _userRepository;

        public ExportOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IUserRepository userRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._userRepository = userRepository;
        }

        public async Task<byte[]> Handle(ExportOrderRequestsQuery request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var user = await this._userRepository.GetUserByIdAsync(request.UserId);
            var role = user!.UserRoles.Select(x => x.Role.RoleName).FirstOrDefault();

            var (supervisorId, vendedorId) = this.GetRoleFilters(role!, request.UserId);
            var orderRequests = await this._orderRequestRepository.GetAllAsync(
                request.ClientCode,
                request.OrderStatusId,
                request.ReasonRequestId,
                request.StartDate,
                request.EndDate,
                supervisorId,
                vendedorId);

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Solicitud");

            worksheet.Cells[1, 1, 1, 15].Style.Font.Bold = true;
            worksheet.Cells[1, 1, 1, 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[1, 1, 1, 15].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D3D3D3"));
            worksheet.Cells[1, 1, 1, 15].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, Color.Black);

            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Supervisor";
            worksheet.Cells[1, 3].Value = "Sucursal";
            worksheet.Cells[1, 4].Value = "Tipo de solicitud";
            worksheet.Cells[1, 5].Value = "Fecha de negociación";
            worksheet.Cells[1, 6].Value = "Ventana de tiempo";
            worksheet.Cells[1, 7].Value = "Motivo de retiro";
            worksheet.Cells[1, 8].Value = "Código de cliente";
            worksheet.Cells[1, 9].Value = "Cliente";
            worksheet.Cells[1, 10].Value = "Observación";
            worksheet.Cells[1, 11].Value = "Referencia";
            worksheet.Cells[1, 12].Value = "Tamaño de Activo";
            worksheet.Cells[1, 13].Value = "Estado";
            worksheet.Cells[1, 14].Value = "Creación de solicitud";
            worksheet.Cells[1, 15].Value = "Actualización de solicitud";

            var row = 2;
            foreach (var order in orderRequests)
            {
                worksheet.Cells[row, 1].Value = order.OrderRequestId;
                worksheet.Cells[row, 2].Value = $"{order.Supervisor.PaternalSurName} {order.Supervisor.MaternalSurName} {order.Supervisor.Names}";
                worksheet.Cells[row, 3].Value = order.Sucursal.CediName;
                worksheet.Cells[row, 4].Value = order.ReasonRequest.ReasonDescription;
                worksheet.Cells[row, 5].Value = order.NegotiatedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                worksheet.Cells[row, 6].Value = order.TimeWindow.TimeRange;
                worksheet.Cells[row, 7].Value = order.WithDrawalReasonId != null
                    ? order.WithDrawalReason!.WithDrawalReasonDescription
                    : "N/A";
                worksheet.Cells[row, 8].Value = order.ClientCode;
                worksheet.Cells[row, 9].Value = order.Client.CompanyName;
                worksheet.Cells[row, 10].Value = order.Observations;
                worksheet.Cells[row, 11].Value = order.Reference;
                worksheet.Cells[row, 12].Value = order.ProductSize.ProductSizeDescription;
                worksheet.Cells[row, 13].Value = order.OrderStatus.StatusName;
                worksheet.Cells[row, 14].Value = order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                worksheet.Cells[row, 15].Value = order.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                row++;
            }

            var dataRange = worksheet.Cells[1, 1, row - 1, 15];
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

        private (int? supervisorId, int? vendedorId) GetRoleFilters(string role, int userId)
        {
            return role switch
            {
                RolesConst.Administrador or RolesConst.Jefe or RolesConst.ProveedorLogistico or RolesConst.Trade => (null, null),
                RolesConst.Supervisor => (supervisorId: userId, vendedorId: null),
                RolesConst.Vendedor => (supervisorId: null, vendedorId: userId),
                _ => (supervisorId: null, vendedorId: null),
            };
        }
    }
}
