namespace BackEndAje.Api.Application.OrderRequestAssets.Commands.AssignAssetsToOrderRequest
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class AssignAssetsToOrderRequestHandler : IRequestHandler<AssignAssetsToOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IAssetRepository _assetRepository;

        public AssignAssetsToOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IClientAssetRepository clientAssetRepository, IAssetRepository assetRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._clientAssetRepository = clientAssetRepository;
            this._assetRepository = assetRepository;
        }

        public async Task<Unit> Handle(AssignAssetsToOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = await this._orderRequestRepository.GetOrderRequestById(request.OrderRequestId);
            foreach (var assetId in request.AssetIds)
            {
                var asset = await this._assetRepository.GetAssetById(assetId);
                var orderRequestAssetId = await this._orderRequestRepository.AssignAssetToOrder(request.OrderRequestId, assetId, request.AssignedBy);
                var newClientAsset = new ClientAssets()
                {
                    CediId = orderRequest!.CediId,
                    InstallationDate = null,
                    ClientId = orderRequest.ClientId,
                    AssetId = assetId,
                    CodeAje = asset.CodeAje,
                    Notes = "Activo asignado a un cliente pero no Aprobado",
                    IsActive = null,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.AssignedBy,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = request.AssignedBy,
                };
                await this._clientAssetRepository.AddClientAsset(newClientAsset);

                var traceEntry = new OrderRequestAssetsTrace
                {
                    OrderRequestAssetId = orderRequestAssetId,
                    OrderRequestId = request.OrderRequestId,
                    AssetId = assetId,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.AssignedBy,
                };

                await this._orderRequestRepository.AddOrderRequestAssetTrace(traceEntry);
            }

            return Unit.Value;
        }
    }
}