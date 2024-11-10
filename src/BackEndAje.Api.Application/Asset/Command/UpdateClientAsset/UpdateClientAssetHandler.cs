namespace BackEndAje.Api.Application.Asset.Command.UpdateClientAsset
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateClientAssetHandler : IRequestHandler<UpdateClientAssetCommand, Unit>
    {
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IMapper _mapper;

        public UpdateClientAssetHandler(IMapper mapper, IClientAssetRepository clientAssetRepository)
        {
            this._mapper = mapper;
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<Unit> Handle(UpdateClientAssetCommand request, CancellationToken cancellationToken)
        {
            var existingClientAsset = await this._clientAssetRepository.GetClientAssetByIdAsync(request.ClientAssetId);
            if (existingClientAsset == null)
            {
                throw new InvalidOperationException($"Client con Activo asociado no existe.");
            }

            var newClientAsset = this._mapper.Map<ClientAssets>(request);
            newClientAsset.AssetId = existingClientAsset.AssetId;
            newClientAsset.CodeAje = existingClientAsset.CodeAje;
            newClientAsset.ClientId = existingClientAsset.ClientId;
            newClientAsset.IsActive = existingClientAsset.IsActive;
            newClientAsset.CreatedAt = existingClientAsset.CreatedAt;
            newClientAsset.CreatedBy = existingClientAsset.CreatedBy;
            await this._clientAssetRepository.UpdateClientAssetsAsync(newClientAsset);
            return Unit.Value;
        }
    }
}