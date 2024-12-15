namespace BackEndAje.Api.Application.Services
{
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;

    public class RelocationService
    {
        private readonly IOrderRequestRepository _orderRequestRepository;

        public RelocationService(IOrderRequestRepository orderRequestRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
        }

        public async Task<Relocation> CreateRelocationAsync(CreateOrderRequestsCommand request, int assetId)
        {
            var relocation = new Relocation
            {
                OriginClientId = request.ClientId,
                DestinationClientId = request.DestinationClientId!.Value,
                TransferredAssetId = assetId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
            };

            await this._orderRequestRepository.AddRelocation(relocation);
            return relocation;
        }

        public async Task CreateRelocationRequestsAsync(Relocation relocation, OrderRequest withdrawalOrderRequest, OrderRequest installationOrderRequest, CreateOrderRequestsCommand request)
        {
            await this._orderRequestRepository.AddRelocationRequests(new RelocationRequest
            {
                RelocationId = relocation.RelocationId,
                ReasonRequestId = 2, // Retiro
                OrderRequestId = withdrawalOrderRequest.OrderRequestId,
                OrderStatusId = withdrawalOrderRequest.OrderStatusId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
            });

            await this._orderRequestRepository.AddRelocationRequests(new RelocationRequest
            {
                RelocationId = relocation.RelocationId,
                ReasonRequestId = 1, // Instalación
                OrderRequestId = installationOrderRequest.OrderRequestId,
                OrderStatusId = installationOrderRequest.OrderStatusId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
            });
        }
    }
}

