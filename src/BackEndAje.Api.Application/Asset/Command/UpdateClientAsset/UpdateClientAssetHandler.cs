using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Application.Asset.Command.UpdateClientAsset
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateClientAssetHandler : IRequestHandler<UpdateClientAssetCommand, Unit>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public UpdateClientAssetHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateClientAssetCommand request, CancellationToken cancellationToken)
        {
            var existingClientAsset = await this._assetRepository.GetClientAssetByIdAsync(request.ClientAssetId);
            if (existingClientAsset == null)
            {
                throw new InvalidOperationException($"Client con Activo asociado no existe.");
            }

            var existingAsset = await this._assetRepository.GetAssetByCodeAje(request.CodeAje);

            var newClientAsset = this._mapper.Map<ClientAssets>(request);
            newClientAsset.AssetId = existingAsset.FirstOrDefault(x => x.IsActive)!.AssetId;
            newClientAsset.CreatedAt = existingClientAsset.CreatedAt;
            newClientAsset.CreatedBy = existingClientAsset.CreatedBy;
            await this._assetRepository.UpdateClientAssetsAsync(newClientAsset);
            return Unit.Value;
        }
    }
}