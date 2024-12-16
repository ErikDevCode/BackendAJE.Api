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
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssetRepository _assetRepository;

        public OrderService(IOrderRequestRepository orderRequestRepository, IMapper mapper, IMastersRepository mastersRepository, IClientAssetRepository clientAssetRepository, IUserRepository userRepository, IClientRepository clientRepository, IAssetRepository assetRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
            this._mastersRepository = mastersRepository;
            this._clientAssetRepository = clientAssetRepository;
            this._userRepository = userRepository;
            this._clientRepository = clientRepository;
            this._assetRepository = assetRepository;
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
            int createdBy,
            OrderRequest orderRequest)
        {
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

            if (orderRequest.ReasonRequestId == 1)
            {
                var clientAsset = await this._clientAssetRepository.GetClientAssetPendingApprovalByClientIdAndAssetIdAsync(orderRequest!.ClientId, assetId);
                var client = await this._clientRepository.GetClientById(orderRequest.ClientId);
                var user = await this._userRepository.GetUserByRouteAsync(client!.Route);
                var asset = await this._assetRepository.GetAssetById(assetId);

                var clientAssetDto = new ClientAssets
                {
                    CediId = user!.CediId!.Value,
                    InstallationDate = clientAsset.InstallationDate,
                    ClientId = orderRequest.ClientId,
                    AssetId = assetId,
                    CodeAje = asset.CodeAje,
                    Notes = "Activo en proceso de reubicación",
                    IsActive = null,
                };

                await this._clientAssetRepository.AddClientAsset(clientAssetDto);
            }
        }
    }
}

