namespace BackEndAje.Api.Application.OrderRequests.Commands.BulkInsertOrderRequests
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class BulkInsertOrderRequestsHandler : IRequestHandler<BulkInsertOrderRequestsCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICediRepository _cediRepository;
        private readonly IMastersRepository _mastersRepository;
        private readonly IClientRepository _clientRepository;

        public BulkInsertOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IUserRepository userRepository, ICediRepository cediRepository, IMastersRepository mastersRepository, IClientRepository clientRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._userRepository = userRepository;
            this._cediRepository = cediRepository;
            this._mastersRepository = mastersRepository;
            this._clientRepository = clientRepository;
        }

        public async Task<Unit> Handle(BulkInsertOrderRequestsCommand request, CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream(request.File);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];

            var rows = worksheet.Dimension.Rows;
            var orderRequests = new List<OrderRequest>();
            var processedRows = new HashSet<string>();

            for (var row = 2; row <= rows; row++)
            {
                var documentNumber = worksheet.Cells[row, 1].Text;
                var cediName = worksheet.Cells[row, 2].Text;
                var reasonDescription = worksheet.Cells[row, 3].Text;
                var negotiatedDate = DateTime.Parse(worksheet.Cells[row, 4].Text).Date;
                var clientCode = worksheet.Cells[row, 7].Text;

                var uniqueKey = $"{documentNumber}-{cediName}-{reasonDescription}-{negotiatedDate}-{clientCode}";
                if (!processedRows.Add(uniqueKey))
                {
                    continue;
                }

                var supervisor = await this._userRepository.GetUserByDocumentNumberAsync(documentNumber);
                if (supervisor == null)
                {
                    throw new KeyNotFoundException($"Supervisor con DNI '{documentNumber}' no encontrado en la fila {row}.");
                }

                var cedi = await this._cediRepository.GetCediByNameAsync(cediName);
                if (cedi == null)
                {
                    throw new KeyNotFoundException($"Sucursal con nombre '{cediName}' no encontrado en la fila {row}.");
                }

                var reasonRequest = await this._mastersRepository.GetReasonRequestByDescriptionAsync(reasonDescription);
                if (reasonRequest == null)
                {
                    throw new KeyNotFoundException($"Tipo de Solicitud con nombre '{cediName}' no encontrado en la fila {row}.");
                }

                var client = await this._clientRepository.GetClientByClientCode(int.Parse(clientCode), cedi.CediId);
                if (client == null)
                {
                    throw new KeyNotFoundException($"Codigo del cliente '{cediName}' no encontrado en la fila {row}.");
                }

                var exists = await this._orderRequestRepository.ExistsAsync(reasonRequest.ReasonRequestId, client.ClientId, negotiatedDate);
                if (exists)
                {
                    continue;
                }

                var timeRange = worksheet.Cells[row, 5].Text;
                var timeWindows = await this._mastersRepository.GetTimeWindowsByTimeRangeAsync(timeRange);
                if (timeWindows == null)
                {
                    throw new KeyNotFoundException($"Ventana de tiempo '{cediName}' no encontrado en la fila {row}.");
                }

                var withDrawalReasonDescription = worksheet.Cells[row, 6].Text;
                int? withDrawalReasonId = null;
                if (!string.IsNullOrWhiteSpace(withDrawalReasonDescription))
                {
                    var withDrawalReason = await this._mastersRepository.GetWithDrawalReasonsByDescriptionAsync(withDrawalReasonDescription);
                    if (withDrawalReason == null)
                    {
                        throw new KeyNotFoundException($"Motivo de retiro '{withDrawalReasonDescription}' no encontrado en la fila {row}.");
                    }

                    withDrawalReasonId = withDrawalReason.WithDrawalReasonId;
                }

                var productSizeDescription = worksheet.Cells[row, 10].Text;
                var productSize = await this._mastersRepository.GetProductSizeByDescriptionAsync(productSizeDescription);
                if (productSize == null)
                {
                    throw new KeyNotFoundException($"Tamaño de producto '{cediName}' no encontrado en la fila {row}.");
                }

                var statusName = worksheet.Cells[row, 11].Text;
                var orderStatus = await this._mastersRepository.GetOrderStatusByNameAsync(statusName);
                if (orderStatus == null)
                {
                    throw new KeyNotFoundException($"Estado '{cediName}' no encontrado en la fila {row}.");
                }

                var orderRequest = new OrderRequest
                {
                    SupervisorId = supervisor.UserId,
                    CediId = cedi.CediId,
                    ReasonRequestId = reasonRequest.ReasonRequestId,
                    NegotiatedDate = negotiatedDate,
                    TimeWindowId = timeWindows.TimeWindowId,
                    WithDrawalReasonId = withDrawalReasonId,
                    ClientId = client.ClientId,
                    ClientCode = int.Parse(clientCode),
                    Observations = worksheet.Cells[row, 8].Text,
                    Reference = worksheet.Cells[row, 9].Text,
                    ProductSizeId = productSize.ProductSizeId,
                    OrderStatusId = orderStatus.OrderStatusId,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                };

                orderRequests.Add(orderRequest);
            }

            await this._orderRequestRepository.BulkInsertOrderRequestsAsync(orderRequests);

            return Unit.Value;
        }
    }
}

