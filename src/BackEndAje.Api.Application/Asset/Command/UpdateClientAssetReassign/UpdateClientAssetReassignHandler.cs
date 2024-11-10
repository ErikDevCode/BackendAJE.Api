namespace BackEndAje.Api.Application.Asset.Command.UpdateClientAssetReassign
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateClientAssetReassignHandler: IRequestHandler<UpdateClientAssetReassignCommand, Unit>
    {
        private readonly IClientAssetRepository _clientAssetRepository;

        public UpdateClientAssetReassignHandler(IClientAssetRepository clientAssetRepository)
        {
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<Unit> Handle(UpdateClientAssetReassignCommand request, CancellationToken cancellationToken)
        {
            var existingClientAsset = await this._clientAssetRepository.GetClientAssetByIdAsync(request.ClientAssetId);
            if (existingClientAsset == null)
            {
                throw new InvalidOperationException("El activo con el cliente asociado no existe.");
            }

            if (existingClientAsset.IsActive)
            {
                throw new InvalidOperationException("No se puede reasignar un Activo que está activo. Debe desactivarse primero.");
            }

            var traceabilityRecord = new ClientAssetsTrace
            {
                ClientAssetId = request.ClientAssetId,
                PreviousClientId = existingClientAsset.ClientId,
                NewClientId = request.NewClientId,
                AssetId = existingClientAsset.AssetId,
                CodeAje = existingClientAsset.CodeAje,
                ChangeReason = request.Notes,
                CreatedAt = DateTime.Now,
                CreatedBy = request.UpdatedBy,
            };
            await this._clientAssetRepository.AddTraceabilityRecordAsync(traceabilityRecord);

            existingClientAsset.ClientId = request.NewClientId;
            existingClientAsset.UpdatedBy = request.UpdatedBy;
            existingClientAsset.UpdatedAt = DateTime.Now;
            existingClientAsset.Notes = request.Notes;
            existingClientAsset.IsActive = true;

            await this._clientAssetRepository.UpdateClientAssetsAsync(existingClientAsset);

            return Unit.Value;
        }
    }
}

