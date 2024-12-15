namespace BackEndAje.Api.Application.Services
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.OrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;

    public class OrderService
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;
        private readonly IMastersRepository _mastersRepository;

        public OrderService(IOrderRequestRepository orderRequestRepository, IMapper mapper, IMastersRepository mastersRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
            this._mastersRepository = mastersRepository;
        }

        public async Task<OrderRequest> CreateOrderRequestAsync(CreateOrderRequestsCommand request)
        {
            var orderRequest = this._mapper.Map<OrderRequest>(request);
            var status = await this._mastersRepository.GetAllOrderStatus();
            var statusInitial = status.FirstOrDefault(x => x.StatusName == "GENERADO")!.OrderStatusId;

            orderRequest.OrderStatusId = statusInitial;
            orderRequest.IsActive = true;

            await this._orderRequestRepository.AddOrderRequestAsync(orderRequest);

            return orderRequest;
        }

        public async Task SaveOrderRequestStatusHistoryAsync(int orderRequestId, int orderStatusId, int createdBy)
        {
            var orderRequestStatusHistory = new OrderRequestStatusHistory
            {
                OrderRequestId = orderRequestId,
                OrderStatusId = orderStatusId,
                ChangeReason = null,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy,
            };

            await this._orderRequestRepository.AddOrderRequestStatusHistoryAsync(orderRequestStatusHistory);
        }

        public async Task SaveOrderRequestDocumentsAsync(int orderRequestId, List<CreateOrderRequestDocumentDto> documents, int createdBy, int updatedBy)
        {
            var orderRequestDocuments = this._mapper.Map<List<Domain.Entities.OrderRequestDocument>>(documents);

            foreach (var document in orderRequestDocuments)
            {
                document.OrderRequestId = orderRequestId;
                document.IsActive = true;
                document.CreatedBy = createdBy;
                document.UpdatedBy = updatedBy;

                await this._orderRequestRepository.AddOrderRequestDocumentAsync(document);
            }
        }

        public async Task SaveOrderRequestAssetsAsync(
            int orderRequestId,
            int assetId,
            int createdBy)
        {
            // Crear el registro en OrderRequestAssets
            var orderRequestAsset = new OrderRequestAssets
            {
                OrderRequestId = orderRequestId,
                AssetId = assetId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy
            };

            var orderRequestAssetId = await this._orderRequestRepository.AssignAssetToOrder(orderRequestId, assetId, createdBy);

            // Crear el registro en OrderRequestAssetsTrace
            var orderRequestAssetTrace = new OrderRequestAssetsTrace
            {
                OrderRequestAssetId = orderRequestAssetId,
                OrderRequestId = orderRequestId,
                AssetId = assetId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy,
            };

            await this._orderRequestRepository.AddOrderRequestAssetTrace(orderRequestAssetTrace);
        }
    }
}

