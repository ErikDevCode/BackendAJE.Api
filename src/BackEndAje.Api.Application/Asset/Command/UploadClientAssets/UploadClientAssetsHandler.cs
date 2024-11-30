namespace BackEndAje.Api.Application.Asset.Command.UploadClientAssets
{
    using System.Globalization;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class UploadClientAssetsHandler : IRequestHandler<UploadClientAssetsCommand, Unit>
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

        public async Task<Unit> Handle(UploadClientAssetsCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var memoryStream = new MemoryStream(request.FileBytes);
            using var package = new ExcelPackage(memoryStream);
            var worksheet = package.Workbook.Worksheets[0];

            var processedAssets = 0;
            var errors = new List<string>();

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
                    var cedi = await this._cediRepository.GetCediByNameAsync(cediName);
                    if (cedi == null)
                    {
                        errors.Add($"Fila {row}: Sucursal '{cediName}' no encontrada.");
                        continue;
                    }

                    // Validar existencia del Cliente
                    var client = await this._clientRepository.GetClientByClientCode(int.Parse(clientCode), cedi.CediId);
                    if (client == null)
                    {
                        errors.Add($"Fila {row}: Cliente con código '{clientCode}' no encontrado.");
                        continue;
                    }

                    // Validar existencia del Activo
                    var asset = await this._assetRepository.GetAssetByCodeAje(codeAje);
                    if (asset == null)
                    {
                        errors.Add($"Fila {row}: Activo con código Aje '{codeAje}' no encontrado.");
                        continue;
                    }

                    // Validar Fecha de Instalación
                    DateTime? installationDate = null;
                    if (!string.IsNullOrWhiteSpace(installationDateText))
                    {
                        installationDate = DateTime.ParseExact(installationDateText, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    // Validar si ya existe un registro idéntico
                    var existingClientAsset = await this._clientAssetRepository.GetClientAssetByAssetId(asset.FirstOrDefault()!.AssetId);
                    if (existingClientAsset.Count > 0)
                    {
                        foreach (var assets in existingClientAsset)
                        {
                            if (assets.ClientId == client.ClientId)
                            {
                                errors.Add($"Fila {row}: Activo con código Aje '{codeAje}'  registro encontrado.");
                            }
                            else
                            {
                                assets.IsActive = false;
                                assets.UpdatedAt = DateTime.Now;
                                assets.UpdatedBy = request.UpdatedBy;
                                await this._clientAssetRepository.UpdateClientAssetsAsync(assets);

                                // Crear nuevo registro
                                var newClientAsset = new ClientAssets
                                {
                                    CediId = cedi.CediId,
                                    InstallationDate = installationDate,
                                    ClientId = client.ClientId,
                                    AssetId = asset.FirstOrDefault()!.AssetId,
                                    CodeAje = codeAje,
                                    Notes = notes,
                                    IsActive = true,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = request.CreatedBy,
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = request.UpdatedBy,
                                };
                                await this._clientAssetRepository.AddClientAsset(newClientAsset);

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
                                await this._clientAssetRepository.AddTraceabilityRecordAsync(trace);
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
                            AssetId = asset.FirstOrDefault()!.AssetId,
                            CodeAje = codeAje,
                            Notes = notes,
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            CreatedBy = request.CreatedBy,
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = request.UpdatedBy,
                        };
                        await this._clientAssetRepository.AddClientAsset(newClientAsset);

                        // Guardar trazabilidad para el nuevo registro
                        var newTrace = new ClientAssetsTrace
                        {
                            ClientAssetId = newClientAsset.ClientAssetId,
                            PreviousClientId = null,
                            NewClientId = client.ClientId,
                            AssetId = asset.FirstOrDefault()!.AssetId,
                            CodeAje = codeAje,
                            ChangeReason = "Asignación a nuevo cliente",
                            IsActive = true,
                            CreatedBy = request.CreatedBy,
                            CreatedAt = DateTime.Now,
                        };
                        await this._clientAssetRepository.AddTraceabilityRecordAsync(newTrace);

                        processedAssets++;
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Error en la fila {row}: {ex.Message}");
                }
            }

            if (errors.Any())
            {
                throw new Exception($"Errores en la carga:\n{string.Join(Environment.NewLine, errors)}");
            }

            return Unit.Value;
        }
    }
}
