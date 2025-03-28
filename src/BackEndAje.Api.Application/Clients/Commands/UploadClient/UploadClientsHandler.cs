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

            var clientsFromExcel = new List<Client>();
            var errors = new List<UploadError>();

            using var memoryStream = new MemoryStream(request.FileBytes);
            using var package = new ExcelPackage(memoryStream);
            var worksheet = package.Workbook.Worksheets[0];

            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    var clientCode = int.Parse(worksheet.Cells[row, 1].Text);
                    var route = int.Parse(worksheet.Cells[row, 8].Text);
                    var documentTypeName = worksheet.Cells[row, 3].Text.ToUpper();
                    var paymentMethodName = worksheet.Cells[row, 7].Text.ToUpper();

                    if (!documentTypeDict.TryGetValue(documentTypeName, out var documentType))
                    {
                        throw new KeyNotFoundException($"Tipo de documento '{documentTypeName}' no encontrado en la fila {row}.");
                    }

                    if (!paymentMethodDict.TryGetValue(paymentMethodName, out var paymentMethod))
                    {
                        throw new KeyNotFoundException($"Método de pago '{paymentMethodName}' no encontrado en la fila {row}.");
                    }

                    var client = new Client
                    {
                        ClientCode = clientCode,
                        CompanyName = worksheet.Cells[row, 2].Text,
                        DocumentTypeId = documentType.DocumentTypeId,
                        DocumentNumber = worksheet.Cells[row, 4].Text,
                        Email = worksheet.Cells[row, 5].Text,
                        EffectiveDate = DateTime.Parse(worksheet.Cells[row, 6].Text),
                        PaymentMethodId = paymentMethod.PaymentMethodId,
                        Phone = worksheet.Cells[row, 9].Text,
                        Address = worksheet.Cells[row, 10].Text,
                        DistrictId = worksheet.Cells[row, 11].Text,
                        CoordX = worksheet.Cells[row, 12].Text,
                        CoordY = worksheet.Cells[row, 13].Text,
                        Segmentation = worksheet.Cells[row, 14].Text,
                        Route = route,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        CreatedBy = request.CreatedBy,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = request.UpdatedBy,
                    };

                    clientsFromExcel.Add(client);
                }
                catch (Exception ex)
                {
                    errors.Add(new UploadError { Row = row, Message = ex.Message });
                }
            }

            var existingUsers = await this._userRepository.GetUsersByRoutesAsync(
                clientsFromExcel.Select(c => c.Route).Where(r => r.HasValue).Select(r => r!.Value).Distinct());

            var existingClients = await this._clientRepository.GetClientsByClientCodesAsync(clientsFromExcel.Select(c => c.ClientCode).Distinct());

            var clientsToAdd = new List<Client>();
            var clientsToUpdate = new List<Client>();

            foreach (var client in clientsFromExcel)
            {
                var existingUser = existingUsers.FirstOrDefault(u => u.Route == client.Route);
                if (existingUser == null)
                {
                    errors.Add(new UploadError { Row = client.ClientCode, Message = $"No se encontró usuario con ruta {client.Route}." });
                    continue;
                }

                client.UserId = existingUser.UserId;

                var existingClient = existingClients.FirstOrDefault(c => c.ClientCode == client.ClientCode);
                if (existingClient != null)
                {
                    this._clientRepository.Detach(existingClient);

                    var clientHasChanged = this.HasClientChanged(existingClient, client);
                    var userOrRouteChanged = existingClient.UserId != client.UserId || existingClient.Route != client.Route;
                    var wasInactive = !existingClient.IsActive;

                    if (clientHasChanged || userOrRouteChanged || wasInactive)
                    {
                        existingClient.CompanyName = client.CompanyName;
                        existingClient.DocumentTypeId = client.DocumentTypeId;
                        existingClient.DocumentNumber = client.DocumentNumber;
                        existingClient.Email = client.Email;
                        existingClient.EffectiveDate = client.EffectiveDate;
                        existingClient.PaymentMethodId = client.PaymentMethodId;
                        existingClient.Phone = client.Phone;
                        existingClient.Address = client.Address;
                        existingClient.DistrictId = client.DistrictId;
                        existingClient.CoordX = client.CoordX;
                        existingClient.CoordY = client.CoordY;
                        existingClient.Segmentation = client.Segmentation;

                        existingClient.UserId = client.UserId;
                        existingClient.Route = client.Route;

                        existingClient.IsActive = true;
                        existingClient.UpdatedAt = DateTime.Now;
                        existingClient.UpdatedBy = request.UpdatedBy;

                        clientsToUpdate.Add(existingClient);
                    }
                }
                else
                {
                    clientsToAdd.Add(client);
                }
            }

            if (clientsToAdd.Count != 0)
            {
                await this._clientRepository.AddClientsAsync(clientsToAdd);
            }

            if (clientsToUpdate.Count != 0)
            {
                await this._clientRepository.UpdateClientsAsync(clientsToUpdate);
            }

            return new UploadClientsResult
            {
                Success = !errors.Any(),
                ProcessedClients = clientsFromExcel.Count - errors.Count,
                Errors = errors,
            };
        }

        private bool HasClientChanged(Client existingClient, Client newClient)
        {
            return existingClient.CompanyName != newClient.CompanyName ||
                   existingClient.DocumentTypeId != newClient.DocumentTypeId ||
                   existingClient.DocumentNumber != newClient.DocumentNumber ||
                   existingClient.Email != newClient.Email ||
                   existingClient.EffectiveDate != newClient.EffectiveDate ||
                   existingClient.PaymentMethodId != newClient.PaymentMethodId ||
                   existingClient.Phone != newClient.Phone ||
                   existingClient.Address != newClient.Address ||
                   existingClient.DistrictId != newClient.DistrictId ||
                   existingClient.CoordX != newClient.CoordX ||
                   existingClient.CoordY != newClient.CoordY ||
                   existingClient.Segmentation != newClient.Segmentation;
        }
    }
}
