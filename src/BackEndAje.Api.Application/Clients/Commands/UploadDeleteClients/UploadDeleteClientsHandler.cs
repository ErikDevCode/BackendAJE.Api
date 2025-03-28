namespace BackEndAje.Api.Application.Clients.Commands.UploadDeleteClients
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class UploadDeleteClientsHandler : IRequestHandler<UploadDeleteClientsCommand, UploadDeleteClientsResult>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientAssetRepository _clientAssetRepository;

        public UploadDeleteClientsHandler(
            IClientRepository clientRepository,
            IClientAssetRepository clientAssetRepository)
        {
            this._clientRepository = clientRepository;
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<UploadDeleteClientsResult> Handle(UploadDeleteClientsCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var result = new UploadDeleteClientsResult();
            var errors = new List<UploadError>();
            var updatedCount = 0;

            try
            {
                // 1. Leer clientes desde Excel
                var excelClientKeys = new List<(int ClientCode, string CompanyName)>();
                using var memoryStream = new MemoryStream(request.FileBytes);
                using var package = new ExcelPackage(memoryStream);
                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    try
                    {
                        var clientCodeText = worksheet.Cells[row, 1].Text.Trim();
                        var companyName = worksheet.Cells[row, 2].Text.Trim();

                        if (string.IsNullOrWhiteSpace(clientCodeText) || string.IsNullOrWhiteSpace(companyName))
                            continue;

                        if (!int.TryParse(clientCodeText, out var clientCode))
                            throw new Exception($"El código '{clientCodeText}' no es válido (fila {row}).");

                        excelClientKeys.Add((clientCode, companyName));
                    }
                    catch (Exception exFila)
                    {
                        errors.Add(new UploadError
                        {
                            Row = row,
                            Message = exFila.Message,
                        });
                    }
                }

                // 2. Obtener todos los clientes
                var allClients = await this._clientRepository.GetClientsOnlyList();
                var clientDict = allClients.ToDictionary(c => (c.ClientCode, c.CompanyName), c => c);

                // 3. Procesar solo los que vienen en el Excel
                var clientsToDeactivate = new List<Client>();

                foreach (var (clientCode, companyName) in excelClientKeys)
                {
                    try
                    {
                        if (!clientDict.TryGetValue((clientCode, companyName), out var client))
                            throw new Exception($"No se encontró el cliente con código {clientCode} y nombre '{companyName}'.");

                        client.IsActive = false;
                        client.UpdatedAt = DateTime.Now;
                        client.UpdatedBy = request.UpdatedBy;

                        var relatedAssets = await this._clientAssetRepository.GetClientAssetsByClientId(client.ClientId);
                        await this._clientAssetRepository.DeleteRangeAsync(relatedAssets);

                        clientsToDeactivate.Add(client);
                        updatedCount++;
                    }
                    catch (Exception exCliente)
                    {
                        errors.Add(new UploadError
                        {
                            Row = 0,
                            Message = exCliente.Message,
                        });
                    }
                }

                // 4. Actualizar clientes desactivados
                await this._clientRepository.UpdateClientsAsync(clientsToDeactivate);

                result.Success = errors.Count == 0;
                result.UpdatedCount = updatedCount;
                result.Errors = errors;
            }
            catch (Exception ex)
            {
                errors.Add(new UploadError
                {
                    Row = 0,
                    Message = ex.Message,
                });

                result.Success = false;
                result.Errors = errors;
            }

            return result;
        }
    }
}

