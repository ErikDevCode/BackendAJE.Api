namespace BackEndAje.Api.Application.Asset.Command.CreateAsset
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateAssetHandler : IRequestHandler<CreateAssetCommand, Unit>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public CreateAssetHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
        {
            const string assetType = "EEFF ACTUAL";
            var existingAsset = await this._assetRepository.GetAssetByCodeAjeAndLogoAndAssetType(request.CodeAje, request.Logo, assetType);
            if (existingAsset != null)
            {
                throw new InvalidOperationException($"Activo '{request.CodeAje}' ya existe actualmente.");
            }

            var newAsset = this._mapper.Map<Asset>(request);
            newAsset.AssetType = assetType;
            await this._assetRepository.AddAsset(newAsset);
            return Unit.Value;
        }
    }
}

