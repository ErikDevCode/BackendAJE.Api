namespace BackEndAje.Api.Application.OrderRequests.Commands.BulkInsertOrderRequests
{
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
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
        private readonly IAssetRepository _assetRepository;
        private readonly IMediator _mediator;

        public BulkInsertOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IUserRepository userRepository, ICediRepository cediRepository, IMastersRepository mastersRepository, IClientRepository clientRepository, IMediator mediator, IAssetRepository assetRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._userRepository = userRepository;
            this._cediRepository = cediRepository;
            this._mastersRepository = mastersRepository;
            this._clientRepository = clientRepository;
            this._mediator = mediator;
            this._assetRepository = assetRepository;
        }

        public async Task<Unit> Handle(BulkInsertOrderRequestsCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var stream = new MemoryStream(request.File);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];

            var rows = worksheet.Dimension.Rows;
            var processedRows = new HashSet<string>();

            for (var row = 2; row <= rows; row++)
            {
                var documentNumber = worksheet.Cells[row, 1].Text;
                var cediName = worksheet.Cells[row, 2].Text;
                var reasonDescription = worksheet.Cells[row, 3].Text;
                var negotiatedDate = DateTime.Parse(worksheet.Cells[row, 4].Text).Date;
                var clientCode = worksheet.Cells[row, 6].Text;

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
                    throw new KeyNotFoundException($"Tipo de Solicitud con nombre '{reasonDescription}' no encontrado en la fila {row}.");
                }

                var client = await this._clientRepository.GetClientByClientCode(int.Parse(clientCode), cedi.CediId);
                if (client == null)
                {
                    throw new KeyNotFoundException($"Codigo del cliente '{clientCode}' no encontrado en la fila {row}.");
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

                var productSizeDescription = worksheet.Cells[row, 9].Text;
                var productSize = await this._mastersRepository.GetProductSizeByDescriptionAsync(productSizeDescription);
                if (productSize == null)
                {
                    throw new KeyNotFoundException($"Tamaño de producto '{cediName}' no encontrado en la fila {row}.");
                }

                switch (request.ReasonRequest)
                {
                    case (int)ReasonRequestConst.Retiro or (int)ReasonRequestConst.CambioDeEquipo:
                    {
                        var withDrawalReasonDescription = worksheet.Cells[row, 11].Text;
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

                        var codeAje = worksheet.Cells[row, 10].Text;
                        int? assetId = null;
                        if (!string.IsNullOrWhiteSpace(codeAje))
                        {
                            var asset = await this._assetRepository.GetAssetByCodeAje(codeAje);
                            if (asset == null)
                            {
                                throw new KeyNotFoundException($"Codigo de Aje '{codeAje}' no encontrado en la fila {row}.");
                            }

                            assetId = asset.FirstOrDefault()!.AssetId;
                        }

                        var newCreateRequestRetireHandler = new CreateOrderRequestsCommand
                        {
                            SupervisorId = supervisor.UserId,
                            CediId = cedi.CediId,
                            ReasonRequestId = reasonRequest.ReasonRequestId,
                            NegotiatedDate = negotiatedDate,
                            TimeWindowId = timeWindows.TimeWindowId,
                            WithDrawalReasonId = withDrawalReasonId,
                            ClientId = client.ClientId,
                            ClientCode = int.Parse(clientCode),
                            Observations = worksheet.Cells[row, 7].Text,
                            Reference = worksheet.Cells[row, 8].Text,
                            ProductSizeId = productSize.ProductSizeId,
                            AssetId = assetId,
                            Documents = [],
                            CreatedBy = request.CreatedBy,
                            UpdatedBy = request.UpdatedBy,
                        };
                        await this._mediator.Send(newCreateRequestRetireHandler, cancellationToken);
                        break;
                    }

                    case (int)ReasonRequestConst.ServicioTecnico:
                    {
                        var codeAje = worksheet.Cells[row, 10].Text;
                        int? assetId = null;
                        if (!string.IsNullOrWhiteSpace(codeAje))
                        {
                            var asset = await this._assetRepository.GetAssetByCodeAje(codeAje);
                            if (asset == null)
                            {
                                throw new KeyNotFoundException($"Codigo de Aje '{codeAje}' no encontrado en la fila {row}.");
                            }

                            assetId = asset.FirstOrDefault() !.AssetId;
                        }

                        var newCreateRequestRetireHandler = new CreateOrderRequestsCommand
                        {
                            SupervisorId = supervisor.UserId,
                            CediId = cedi.CediId,
                            ReasonRequestId = reasonRequest.ReasonRequestId,
                            NegotiatedDate = negotiatedDate,
                            TimeWindowId = timeWindows.TimeWindowId,
                            WithDrawalReasonId = null,
                            ClientId = client.ClientId,
                            ClientCode = int.Parse(clientCode),
                            Observations = worksheet.Cells[row, 7].Text,
                            Reference = worksheet.Cells[row, 8].Text,
                            ProductSizeId = productSize.ProductSizeId,
                            AssetId = assetId,
                            Documents = [],
                            CreatedBy = request.CreatedBy,
                            UpdatedBy = request.UpdatedBy,
                        };
                        await this._mediator.Send(newCreateRequestRetireHandler, cancellationToken);
                        break;
                    }

                    case (int)ReasonRequestConst.Reubicacion:
                    {
                        var withDrawalReasonDescription = worksheet.Cells[row, 11].Text;
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

                        var codeAje = worksheet.Cells[row, 10].Text;
                        int? assetId = null;
                        if (!string.IsNullOrWhiteSpace(codeAje))
                        {
                            var asset = await this._assetRepository.GetAssetByCodeAje(codeAje);
                            if (asset == null)
                            {
                                throw new KeyNotFoundException($"Codigo de Aje '{codeAje}' no encontrado en la fila {row}.");
                            }

                            assetId = asset.FirstOrDefault()!.AssetId;
                        }

                        var destinationCediName = worksheet.Cells[row, 13].Text;
                        var destinationCedi = await this._cediRepository.GetCediByNameAsync(destinationCediName);
                        if (cedi == null)
                        {
                            throw new KeyNotFoundException($"Sucursal con nombre '{destinationCediName}' no encontrado en la fila {row}.");
                        }

                        var destinationClientCode = worksheet.Cells[row, 12].Text;
                        var destinationClient = await this._clientRepository.GetClientByClientCode(int.Parse(destinationClientCode), destinationCedi.CediId);
                        if (client == null)
                        {
                            throw new KeyNotFoundException($"Codigo del cliente '{clientCode}' no encontrado en la fila {row}.");
                        }

                        var newCreateRequestRetireHandler = new CreateOrderRequestsCommand
                        {
                            SupervisorId = supervisor.UserId,
                            CediId = cedi.CediId,
                            ReasonRequestId = reasonRequest.ReasonRequestId,
                            NegotiatedDate = negotiatedDate,
                            TimeWindowId = timeWindows.TimeWindowId,
                            WithDrawalReasonId = withDrawalReasonId,
                            ClientId = client.ClientId,
                            DestinationClientId = destinationClient.ClientId,
                            DestinationCediId = destinationCedi.CediId,
                            ClientCode = int.Parse(clientCode),
                            Observations = worksheet.Cells[row, 7].Text,
                            Reference = worksheet.Cells[row, 8].Text,
                            ProductSizeId = productSize.ProductSizeId,
                            AssetId = assetId,
                            Documents = [],
                            CreatedBy = request.CreatedBy,
                            UpdatedBy = request.UpdatedBy,
                        };
                        await this._mediator.Send(newCreateRequestRetireHandler, cancellationToken);
                        break;
                    }

                    default:
                    {
                        var newCreateRequestHandler = new CreateOrderRequestsCommand
                        {
                            SupervisorId = supervisor.UserId,
                            CediId = cedi.CediId,
                            ReasonRequestId = reasonRequest.ReasonRequestId,
                            NegotiatedDate = negotiatedDate,
                            TimeWindowId = timeWindows.TimeWindowId,
                            WithDrawalReasonId = null,
                            ClientId = client.ClientId,
                            ClientCode = int.Parse(clientCode),
                            Observations = worksheet.Cells[row, 7].Text,
                            Reference = worksheet.Cells[row, 8].Text,
                            ProductSizeId = productSize.ProductSizeId,
                            Documents = [],
                            CreatedBy = request.CreatedBy,
                            UpdatedBy = request.UpdatedBy,
                        };
                        await this._mediator.Send(newCreateRequestHandler, cancellationToken);
                        break;
                    }
                }
            }

            return Unit.Value;
        }
    }
}

