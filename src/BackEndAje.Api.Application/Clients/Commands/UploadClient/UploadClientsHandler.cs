namespace BackEndAje.Api.Application.Clients.Commands.UploadClient
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class UploadClientsHandler : IRequestHandler<UploadClientsCommand, UploadClientsResult>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMastersRepository _mastersRepository;

        public UploadClientsHandler(IClientRepository clientRepository, IMastersRepository mastersRepository, IUserRepository userRepository)
        {
            this._clientRepository = clientRepository;
            this._mastersRepository = mastersRepository;
            this._userRepository = userRepository;
        }

        public async Task<UploadClientsResult> Handle(UploadClientsCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var documentTypes = await this._mastersRepository.GetAllDocumentType();
            var paymentMethods = await this._mastersRepository.GetAllPaymentMethods();

            var documentTypeDict = documentTypes.ToDictionary(dt => dt.DocumentTypeName.ToUpper());
            var paymentMethodDict = paymentMethods.ToDictionary(pm => pm.PaymentMethod.ToUpper());

            using var memoryStream = new MemoryStream(request.FileBytes);
            using var package = new ExcelPackage(memoryStream);
            var worksheet = package.Workbook.Worksheets[0];

            var clientsToAdd = new List<Client>();
            var processedClients = 0;
            var errors = new List<UploadError>();

            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    var clientCode = int.Parse(worksheet.Cells[row, 1].Text);
                    var route = int.Parse(worksheet.Cells[row, 8].Text);
                    var existingUser = await this._userRepository.GetUserByRouteAsync(route);
                    var existingClient = await this._clientRepository.GetClientByClientCode(clientCode, existingUser!.CediId.Value);

                    if (existingClient != null)
                    {
                        this._clientRepository.Detach(existingClient);
                    }

                    var documentTypeName = worksheet.Cells[row, 3].Text.ToUpper();
                    if (!documentTypeDict.TryGetValue(documentTypeName, out var documentType))
                    {
                        throw new KeyNotFoundException($"Tipo de documento '{documentTypeName}' no encontrado en la fila {row}.");
                    }

                    var paymentMethodName = worksheet.Cells[row, 7].Text.ToUpper();
                    if (!paymentMethodDict.TryGetValue(paymentMethodName, out var paymentMethod))
                    {
                        throw new KeyNotFoundException($"Método de pago '{paymentMethodName}' no encontrado en la fila {row}.");
                    }

                    if (existingUser is null)
                    {
                        throw new KeyNotFoundException($"La ruta '{route}' no encontrado en la fila {row}.");
                    }

                    var client = existingClient ?? new Client
                    {
                        ClientCode = clientCode,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        CreatedBy = request.CreatedBy,
                    };

                    client.UserId = existingUser.UserId;
                    client.Route = existingUser.Route;
                    this.UpdateClientData(client, worksheet, row, documentType.DocumentTypeId, paymentMethod.PaymentMethodId, request.UpdatedBy);

                    if (existingClient == null)
                    {
                        clientsToAdd.Add(client);
                    }
                    else
                    {
                        await this._clientRepository.UpdateClientAsync(client);
                    }

                    processedClients++;
                }
                catch (Exception ex)
                {
                    errors.Add(new UploadError
                    {
                        Row = row,
                        Message = ex.Message,
                    });
                }
            }

            if (clientsToAdd.Any())
            {
                await this._clientRepository.AddClientsAsync(clientsToAdd);
            }

            return new UploadClientsResult
            {
                Success = !errors.Any(),
                ProcessedClients = processedClients,
                Errors = errors,
            };
        }

        private void UpdateClientData(Client client, ExcelWorksheet worksheet, int row, int documentTypeId, int paymentMethodId, int updatedBy)
        {
            client.CompanyName = worksheet.Cells[row, 2].Text;
            client.DocumentTypeId = documentTypeId;
            client.DocumentNumber = worksheet.Cells[row, 4].Text;
            client.Email = worksheet.Cells[row, 5].Text;
            client.EffectiveDate = DateTime.Parse(worksheet.Cells[row, 6].Text);
            client.PaymentMethodId = paymentMethodId;
            client.UserId = client.UserId;
            client.Route = client.Route;
            client.Phone = worksheet.Cells[row, 9].Text;
            client.Address = worksheet.Cells[row, 10].Text;
            client.DistrictId = worksheet.Cells[row, 11].Text;
            client.CoordX = worksheet.Cells[row, 12].Text;
            client.CoordY = worksheet.Cells[row, 13].Text;
            client.Segmentation = worksheet.Cells[row, 14].Text;
            client.UpdatedAt = DateTime.Now;
            client.UpdatedBy = updatedBy;
        }
    }
}
