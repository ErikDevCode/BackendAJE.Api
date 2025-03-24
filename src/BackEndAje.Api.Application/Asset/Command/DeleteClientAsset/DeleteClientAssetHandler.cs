namespace BackEndAje.Api.Application.Asset.Command.DeleteClientAsset
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DeleteClientAssetHandler : IRequestHandler<DeleteClientAssetCommand, Unit>
    {
        private readonly IClientAssetRepository _clientAssetRepository;

        public DeleteClientAssetHandler(IClientAssetRepository clientAssetRepository)
        {
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<Unit> Handle(DeleteClientAssetCommand request, CancellationToken cancellationToken)
        {
            var clientAsset = await this._clientAssetRepository.GetClientAssetByIdAsync(request.ClientAssetId);
            if (clientAsset == null)
            {
                throw new KeyNotFoundException($"Client con Activo asociado no existe.");
            }

            await this._clientAssetRepository.DeleteClientAssetAsync(clientAsset);
            return Unit.Value;
        }
    }
}
