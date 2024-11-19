namespace BackEndAje.Api.Application.OrderRequestAssets.Commands.DeleteAssetToOrderRequest
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DeleteAssetToOrderRequestHandler : IRequestHandler<DeleteAssetToOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IClientAssetRepository _clientAssetRepository;

        public DeleteAssetToOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IClientAssetRepository clientAssetRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<Unit> Handle(DeleteAssetToOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequestAsset = await this._orderRequestRepository.GetOrderRequestAssetsById(request.OrderRequestAssetId);
            orderRequestAsset.IsActive = false;
            await this._orderRequestRepository.UpdateAssetToOrderRequest(orderRequestAsset);

            var traceEntry = new OrderRequestAssetsTrace
            {
                OrderRequestAssetId = orderRequestAsset.OrderRequestAssetId,
                OrderRequestId = orderRequestAsset.OrderRequestId,
                AssetId = orderRequestAsset.AssetId,
                IsActive = false,
                CreatedAt = DateTime.Now,
                CreatedBy = request.AssignedBy,
            };

            await this._orderRequestRepository.AddOrderRequestAssetTrace(traceEntry);

            var orderRequest = await this._orderRequestRepository.GetOrderRequestById(orderRequestAsset.OrderRequestId);
            var clientAsset = await this._clientAssetRepository.GetClientAssetByClientIdAndAssetIdAndIsNotActivateAsync(
                    orderRequest!.ClientId, orderRequestAsset.AssetId);

            await this._clientAssetRepository.DeleteClientAssetAsync(clientAsset);

            return Unit.Value;
        }
    }
}

