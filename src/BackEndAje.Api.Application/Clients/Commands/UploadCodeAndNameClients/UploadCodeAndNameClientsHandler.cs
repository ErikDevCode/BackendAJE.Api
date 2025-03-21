namespace BackEndAje.Api.Application.Clients.Commands.UploadCodeAndNameClients
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class UploadCodeAndNameClientsHandler : IRequestHandler<UploadCodeAndNameClientsCommand, UploadCodeAndNameClientsResult>
    {
        private readonly IClientRepository _clientRepository;

        public UploadCodeAndNameClientsHandler(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }

        public async Task<UploadCodeAndNameClientsResult> Handle(UploadCodeAndNameClientsCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var result = new UploadCodeAndNameClientsResult();
            var errors = new List<UploadError>();
            var updatedCount = 0;

            try
            {
                // ---------------------------------------
                // A) TRAER LISTA DE CLIENTES DE LA BD
                // ---------------------------------------
                var allClients = await this._clientRepository.GetClientsOnlyList();
                // Por ej. List<Client>

                // Crear un diccionario para búsqueda rápida
                var clientDict = allClients.ToDictionary(c => c.ClientId, c => c);

                // ---------------------------------------
                // B) LEER EL EXCEL Y GUARDAR DATOS
                // ---------------------------------------
                using var memoryStream = new MemoryStream(request.FileBytes);
                using var package = new ExcelPackage(memoryStream);

                var worksheet = package.Workbook.Worksheets[0];

                // Lista temporal para los registros del Excel
                var excelRows = new List<ExcelRowDto>();

                // Asumiendo fila 1 con encabezados: (ClientId, ClientCode, CompanyName)
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    try
                    {
                        var clientIdText = worksheet.Cells[row, 1].Text.Trim();    // Col A
                        var clientCodeText = worksheet.Cells[row, 2].Text.Trim();  // Col B
                        var companyName = worksheet.Cells[row, 3].Text.Trim();     // Col C

                        if (string.IsNullOrWhiteSpace(clientIdText))
                            continue; // Puede ser una fila en blanco

                        if (!int.TryParse(clientIdText, out var clientId))
                        {
                            throw new Exception($"ClientId '{clientIdText}' no es un número válido (fila {row}).");
                        }

                        excelRows.Add(new ExcelRowDto
                        {
                            RowNumber = row,
                            ClientId = clientId,
                            ClientCodeText = clientCodeText,
                            CompanyName = companyName,
                        });
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

                // ---------------------------------------
                // C) HACER MATCH Y ACTUALIZAR
                // ---------------------------------------
                // Lista de clientes que sí se van a actualizar
                var updatedClients = new List<Client>();

                foreach (var rowDto in excelRows)
                {
                    try
                    {
                        // Verificar si existe en BD
                        if (!clientDict.TryGetValue(rowDto.ClientId, out var existingClient))
                        {
                            // No existe => reportar error o saltar
                            throw new Exception($"No existe cliente con Id={rowDto.ClientId} (fila {rowDto.RowNumber}).");
                        }

                        // Si 'ClientCode' debe ser un entero, parsear:
                        if (!int.TryParse(rowDto.ClientCodeText, out var codeParsed))
                        {
                            throw new Exception($"Código '{rowDto.ClientCodeText}' no es válido (fila {rowDto.RowNumber}).");
                        }

                        // Actualizar campos en memoria
                        existingClient.ClientCode = codeParsed;
                        existingClient.CompanyName = rowDto.CompanyName;
                        existingClient.UpdatedAt = DateTime.Now;
                        existingClient.UpdatedBy = request.UpdatedBy;

                        // Agregar a la lista de actualizados
                        updatedClients.Add(existingClient);
                        updatedCount++;
                    }
                    catch (Exception exMatch)
                    {
                        errors.Add(new UploadError
                        {
                            Row = rowDto.RowNumber,
                            Message = exMatch.Message,
                        });
                    }
                }

                // ---------------------------------------
                // D) GUARDAR CAMBIOS
                // ---------------------------------------
                // Sólo se actualizan los que hicieron match
                await this._clientRepository.UpdateClientsAsync(updatedClients);
            }
            catch (Exception ex)
            {
                // Error global
                errors.Add(new UploadError
                {
                    Row = 0,
                    Message = ex.Message
                });
            }

            // ---------------------------------------
            // E) RETORNAR RESULTADO
            // ---------------------------------------
            result.Success = (errors.Count == 0);
            result.UpdatedCount = updatedCount;
            result.Errors = errors;

            return result;
        }
    }

    // Clase auxiliar para almacenar datos del Excel
    internal class ExcelRowDto
    {
        public int RowNumber { get; set; }

        public int ClientId { get; set; }

        public string ClientCodeText { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
    }
}
