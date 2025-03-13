namespace BackEndAje.Api.Application.Asset.Command.UploadAssets
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class UploadAssetsHandler : IRequestHandler<UploadAssetsCommand, UploadAssetsResult>
    {
        private readonly IAssetRepository _assetRepository;

        public UploadAssetsHandler(IAssetRepository assetRepository)
        {
            this._assetRepository = assetRepository;
        }

        public async Task<UploadAssetsResult> Handle(UploadAssetsCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var memoryStream = new MemoryStream(request.FileBytes);
            using var package = new ExcelPackage(memoryStream);
            var worksheet = package.Workbook.Worksheets[0];

            var assetsToAdd = new List<Domain.Entities.Asset>();
            var processedAssets = 0;
            var errors = new List<UploadError>();

            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    var codeAje = worksheet.Cells[row, 2].Text;
                    var logo = worksheet.Cells[row, 3].Text;
                    var assetType = worksheet.Cells[row, 4].Text;
                    var existingAsset = await this._assetRepository.GetAssetByCodeAjeAndLogoAndAssetType(codeAje, logo, assetType);

                    var asset = existingAsset ?? new Domain.Entities.Asset()
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = request.CreatedBy,
                    };

                    this.UpdateAssetData(asset, worksheet, row, codeAje, logo, assetType, request.UpdatedBy);

                    if (existingAsset == null)
                    {
                        assetsToAdd.Add(asset);
                    }
                    else
                    {
                        await this._assetRepository.UpdateAssetAsync(asset);
                    }

                    processedAssets++;
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

            if (assetsToAdd.Any())
            {
                await this._assetRepository.AddAssetsAsync(assetsToAdd);
            }

            return new UploadAssetsResult
            {
                Success = !errors.Any(),
                ProcessedClients = processedAssets,
                Errors = errors,
            };
        }

        private void UpdateAssetData(Domain.Entities.Asset asset, ExcelWorksheet worksheet, int row, string codeAje, string logo, string assetType,  int updatedBy)
        {
            asset.CodeAje = codeAje;
            asset.Logo = logo;
            asset.AssetType = assetType;
            asset.Brand = worksheet.Cells[row, 5].Text;
            asset.Model = worksheet.Cells[row, 6].Text;
            var isActiveText = worksheet.Cells[row, 7].Text.Trim();
            if (isActiveText is "1" or "0")
            {
                asset.IsActive = isActiveText == "1";
            }
            else
            {
                throw new FormatException($"Valor inválido para IsActive en la fila {row}: '{isActiveText}'. Se esperaba '1' o '0'.");
            }

            asset.UpdatedAt = DateTime.Now;
            asset.UpdatedBy = updatedBy;
        }
    }
}

