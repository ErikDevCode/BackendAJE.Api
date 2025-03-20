namespace BackEndAje.Api.Application.Asset.Command.UploadClientAssets
{
    using System.Globalization;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class UploadClientAssetsHandler : IRequestHandler<UploadClientAssetsCommand, UploadClientAssetResult>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly ICediRepository _cediRepository;
        private readonly IClientRepository _clientRepository;

        public UploadClientAssetsHandler(IAssetRepository assetRepository, IClientAssetRepository clientAssetRepository, ICediRepository cediRepository, IClientRepository clientRepository)
        {
            this._assetRepository = assetRepository;
            this._clientAssetRepository = clientAssetRepository;
            this._cediRepository = cediRepository;
            this._clientRepository = clientRepository;
        }

        public async Task<UploadClientAssetResult> Handle(UploadClientAssetsCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var memoryStream = new MemoryStream(request.FileBytes);
            using var package = new ExcelPackage(memoryStream);
            var worksheet = package.Workbook.Worksheets[0];

            var processedAssets = 0;
            var errors = new List<UploadError>();

            // Cargar datos en memoria
            var cedisList = await this._cediRepository.GetAllCedis();
            var clientsList = await this._clientRepository.GetClientsList();
            var assetsList = await this._assetRepository.GetAssetsList();

            // Listas temporales para registrar las actualizaciones y creaciones
            var clientAssetsToUpdate = new List<ClientAssets>();
            var clientAssetsToAdd = new List<ClientAssets>();
            var traceabilityRecords = new List<ClientAssetsTrace>();

            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    // Leer datos del Excel
                    var cediName = worksheet.Cells[row, 1].Text;
                    var installationDateText = worksheet.Cells[row, 2].Text;
                    var clientCode = worksheet.Cells[row, 3].Text;
                    var codeAje = worksheet.Cells[row, 4].Text;
                    var notes = worksheet.Cells[row, 5].Text;

                    // Validar existencia del Cedi
                    var cedi = cedisList.FirstOrDefault(x => x.CediName.Equals(cediName, StringComparison.CurrentCultureIgnoreCase));
                    if (cedi == null)
                    {
                        errors.Add(new UploadError
                        {
                            Row = row,
                            Message = $"Sucursal '{cediName}' no encontrada.",
                        });
                        continue;
                    }

                    // Validar existencia del Cliente
                    var client = clientsList.FirstOrDefault(x => x.ClientCode == int.Parse(clientCode) && x.Seller.CediId == cedi.CediId);
                    if (client == null)
                    {
                        errors.Add(new UploadError
                        {
                            Row = row,
                            Message = $"Cliente con código '{clientCode}' no encontrado.",
                        });
                        continue;
                    }

                    // Validar existencia del Activo
                    var asset = assetsList.Where(x => x.CodeAje == codeAje).ToList();
                    if (!asset.Any())
                    {
                        errors.Add(new UploadError
                        {
                            Row = row,
                            Message = $"Activo con código Aje '{codeAje}' no encontrado.",
                        });
                        continue;

                    }

                    // Validar Fecha de Instalación
                    DateTime? installationDate = null;
                    if (!string.IsNullOrWhiteSpace(installationDateText))
                    {
                        installationDate = DateTime.ParseExact(installationDateText, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    // Validar si ya existe un registro idéntico
                    var existingClientAsset = await this._clientAssetRepository.GetClientAssetByAssetId(asset.First().AssetId);
                    if (existingClientAsset.Any())
                    {
                        foreach (var assets in existingClientAsset)
                        {
                            if (assets.ClientId == client.ClientId)
                            {
                                errors.Add(new UploadError
                                {
                                    Row = row,
                                    Message = $"Activo con código Aje '{codeAje}' ya asignado a este cliente.",
                                });
                            }
                            else
                            {
                                // Actualizar el registro existente
                                assets.IsActive = false;
                                assets.UpdatedAt = DateTime.Now;
                                assets.UpdatedBy = request.UpdatedBy;
                                clientAssetsToUpdate.Add(assets);

                                // Crear nuevo registro
                                var newClientAsset = new ClientAssets
                                {
                                    CediId = cedi.CediId,
                                    InstallationDate = installationDate,
                                    ClientId = client.ClientId,
                                    AssetId = asset.First().AssetId,
                                    CodeAje = codeAje,
                                    Notes = notes,
                                    IsActive = true,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = request.CreatedBy,
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = request.UpdatedBy,
                                };
                                clientAssetsToAdd.Add(newClientAsset);

                                // Guardar trazabilidad
                                var trace = new ClientAssetsTrace
                                {
                                    ClientAssetId = assets.ClientAssetId,
                                    PreviousClientId = assets.ClientId,
                                    NewClientId = client.ClientId,
                                    AssetId = assets.AssetId,
                                    CodeAje = codeAje,
                                    ChangeReason = "Se realizó asignación a otro cliente",
                                    IsActive = true,
                                    CreatedBy = request.UpdatedBy,
                                    CreatedAt = DateTime.Now,
                                };
                                traceabilityRecords.Add(trace);
                            }
                        }
                    }
                    else
                    {
                        // Crear nuevo registro
                        var newClientAsset = new ClientAssets
                        {
                            CediId = cedi.CediId,
                            InstallationDate = installationDate,
                            ClientId = client.ClientId,
                            AssetId = asset.First().AssetId,
                            CodeAje = codeAje,
                            Notes = notes,
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            CreatedBy = request.CreatedBy,
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = request.UpdatedBy,
                        };
                        clientAssetsToAdd.Add(newClientAsset);

                        // Guardar trazabilidad para el nuevo registro
                        var newTrace = new ClientAssetsTrace
                        {
                            ClientAsset = newClientAsset,
                            PreviousClientId = null,
                            NewClientId = client.ClientId,
                            AssetId = asset.First().AssetId,
                            CodeAje = codeAje,
                            ChangeReason = "Asignación a nuevo cliente",
                            IsActive = true,
                            CreatedBy = request.CreatedBy,
                            CreatedAt = DateTime.Now,
                        };
                        traceabilityRecords.Add(newTrace);

                        processedAssets++;
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(new UploadError
                    {
                        Row = row,
                        Message = $"Error en la fila {row}: {ex.Message}",
                    });
                }
            }

            // Realizar todas las actualizaciones y creaciones al final
            if (clientAssetsToUpdate.Any())
            {
                await this._clientAssetRepository.UpdateClientAssetsListAsync(clientAssetsToUpdate);
            }

            if (clientAssetsToAdd.Any())
            {
                await this._clientAssetRepository.AddClientListAsset(clientAssetsToAdd);
            }

            if (traceabilityRecords.Any())
            {
                await this._clientAssetRepository.AddTraceabilityRecordListAsync(traceabilityRecords);
            }

            return new UploadClientAssetResult
            {
                Success = !errors.Any(),
                ProcessedAssets = processedAssets,
                Errors = errors,
            };
        }
    }
}
