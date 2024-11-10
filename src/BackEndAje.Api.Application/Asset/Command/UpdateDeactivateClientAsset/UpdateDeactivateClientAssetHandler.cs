namespace BackEndAje.Api.Application.Asset.Command.UpdateDeactivateClientAsset
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateDeactivateClientAssetHandler : IRequestHandler<UpdateDeactivateClientAssetCommand, bool>
    {
        private readonly IClientAssetRepository _clientAssetRepository;

        public UpdateDeactivateClientAssetHandler(IClientAssetRepository clientAssetRepository)
        {
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<bool> Handle(UpdateDeactivateClientAssetCommand request, CancellationToken cancellationToken)
        {
            var existingClientAsset = await this._clientAssetRepository.GetClientAssetByIdAsync(request.ClientAssetId);
            if (existingClientAsset == null)
            {
                throw new InvalidOperationException($"Cliente con Activo asociado no existe.");
            }

            if (!existingClientAsset.IsActive)
            {
                var existingActiveCodeAje = await this._clientAssetRepository.GetClientAssetsByCodeAje(existingClientAsset.CodeAje);
                var isCodeAjeActiveWithAnotherClient = existingActiveCodeAje.Any(ca => ca.IsActive && ca.CodeAje == existingClientAsset.CodeAje);

                if (isCodeAjeActiveWithAnotherClient)
                {
                    throw new InvalidOperationException($"No se puede actualizar el estado porque ya hay un cliente activo con el código Aje '{existingClientAsset.CodeAje}'.");
                }
            }

            existingClientAsset.IsActive = existingClientAsset.IsActive is false;
            existingClientAsset.UpdatedBy = request.UpdatedBy;

            await this._clientAssetRepository.UpdateClientAssetsAsync(existingClientAsset);
            return true;
        }
    }
}

