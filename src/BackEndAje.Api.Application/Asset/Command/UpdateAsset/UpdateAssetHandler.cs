namespace BackEndAje.Api.Application.Asset.Command.UpdateAsset
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, Unit>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public UpdateAssetHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            var existingAssets = await this._assetRepository.GetAssetById(request.AssetId);
            if (existingAssets == null)
            {
                throw new InvalidOperationException($"Activo con ID '{request.AssetId}' no existe.");
            }

            var newAsset = this._mapper.Map<Asset>(request);
            newAsset.CreatedAt = existingAssets.CreatedAt;
            newAsset.CreatedBy = existingAssets.CreatedBy;

            var asset = await this._assetRepository.GetAssetByCodeAje(request.CodeAje);
            if (asset.Count != 0)
            {
                throw new InvalidOperationException($"Activo ya se encuentra registrado.");
            }

            await this._assetRepository.UpdateAssetAsync(newAsset);
            return Unit.Value;
        }
    }
}
