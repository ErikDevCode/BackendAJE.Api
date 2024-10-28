namespace BackEndAje.Api.Application.Asset.Command.UpdateStatusAsset
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateStatusAssetHandler : IRequestHandler<UpdateStatusAssetCommand, bool>
    {
        private readonly IAssetRepository _assetRepository;

        public UpdateStatusAssetHandler(IAssetRepository assetRepository)
        {
            this._assetRepository = assetRepository;
        }

        public async Task<bool> Handle(UpdateStatusAssetCommand request, CancellationToken cancellationToken)
        {
            var existingAsset = await this._assetRepository.GetAssetById(request.AssetId);
            if (existingAsset == null)
            {
                throw new InvalidOperationException($"Activo con ID '{request.AssetId}' no existe.");
            }

            existingAsset.IsActive = existingAsset.IsActive is false;
            existingAsset.UpdatedBy = request.UpdatedBy;
            await this._assetRepository.UpdateAssetAsync(existingAsset);
            return true;
        }
    }
}
