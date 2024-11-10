namespace BackEndAje.Api.Application.Asset.Command.CreateClientAsset
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateClientAssetHandler : IRequestHandler<CreateClientAssetCommand, Unit>
    {
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public CreateClientAssetHandler(IMapper mapper, IClientAssetRepository clientAssetRepository, IAssetRepository assetRepository)
        {
            this._mapper = mapper;
            this._clientAssetRepository = clientAssetRepository;
            this._assetRepository = assetRepository;
        }

        public async Task<Unit> Handle(CreateClientAssetCommand request, CancellationToken cancellationToken)
        {
            var clientAsset = await this._clientAssetRepository.GetClientAssetsByCodeAje(request.CodeAje);
            if (clientAsset.Any())
            {
                throw new InvalidOperationException($"Codigo Aje: '{request.CodeAje}' tiene un Cliente vigente asignado actulmente, primero debe desasignar el cliente anterior");
            }

            var asset = await this._assetRepository.GetAssetByCodeAje(request.CodeAje);

            if (!asset.Any())
            {
                throw new InvalidOperationException($"Codigo Aje: '{request.CodeAje}' no existe en los registros de Activos debe cargarlo antes de relacionarlo.");
            }

            var newClientAsset = this._mapper.Map<ClientAssets>(request);
            newClientAsset.AssetId = asset.FirstOrDefault(x => x.IsActive=true)!.AssetId;
            await this._clientAssetRepository.AddClientAsset(newClientAsset);
            return Unit.Value;
        }
    }
}

