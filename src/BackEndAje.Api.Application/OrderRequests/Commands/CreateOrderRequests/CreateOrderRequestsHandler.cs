namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.Services;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateOrderRequestsHandler : IRequestHandler<CreateOrderRequestsCommand, Unit>
    {
        private readonly OrderService _orderService;
        private readonly NotificationNewService _notificationNewService;
        private readonly RelocationService _relocationService;
        private readonly IClientRepository _clientRepository;

        public CreateOrderRequestsHandler(
            OrderService orderService,
            NotificationNewService notificationNewService,
            RelocationService relocationService,
            IClientRepository clientRepository)
        {
            this._orderService = orderService;
            this._notificationNewService = notificationNewService;
            this._relocationService = relocationService;
            this._clientRepository = clientRepository;
        }

        public async Task<Unit> Handle(CreateOrderRequestsCommand request, CancellationToken cancellationToken)
        {
            if (request.ReasonRequestId == (int)ReasonRequestConst.Reubicacion)
            {
                this.ValidateRelocationRequest(request);

                // Crear orden de retiro
                var withdrawalOrderRequest = await this.CreateOrderAndNotifyAsync(
                    request with { ReasonRequestId = (int)ReasonRequestConst.Retiro },
                    cancellationToken,
                    request.ReasonRequestId);

                await this._orderService.SaveOrderRequestAssetsAsync(withdrawalOrderRequest.OrderRequestId, request.AssetId!.Value, request.CreatedBy, withdrawalOrderRequest);

                // Crear orden de instalación
                var client = await this._clientRepository.GetClientById(request.DestinationClientId!.Value);
                var installationOrderRequest = await this.CreateOrderAndNotifyAsync(
                    request with { ReasonRequestId = (int)ReasonRequestConst.Instalacion, ClientId = client!.ClientId, ClientCode = client.ClientCode, CediId = (int)request.DestinationCediId! },
                    cancellationToken,
                    request.ReasonRequestId);

                await this._orderService.SaveOrderRequestAssetsAsync(installationOrderRequest.OrderRequestId, request.AssetId!.Value, request.CreatedBy, installationOrderRequest);

                // Crear Relocation y asociar RelocationRequests
                var relocation = await this._relocationService.CreateRelocationAsync(request, request.AssetId!.Value);
                await this._relocationService.CreateRelocationRequestsAsync(relocation, withdrawalOrderRequest, installationOrderRequest, request);
            }
            else
            {
                await this.CreateOrderAndNotifyAsync(request, cancellationToken, request.ReasonRequestId);
            }

            return Unit.Value;
        }

        private void ValidateRelocationRequest(CreateOrderRequestsCommand request)
        {
            if (!request.DestinationClientId.HasValue || !request.AssetId.HasValue)
            {
                throw new ArgumentException("DestinationClientId y AssetId son obligatorios para una reubicación.");
            }
        }

        private async Task<OrderRequest> CreateOrderAndNotifyAsync(
            CreateOrderRequestsCommand request,
            CancellationToken cancellationToken,
            int reasonRequestId)
        {
            var orderRequest = await this._orderService.CreateOrderRequestAsync(request);
            var client = await this._clientRepository.GetClientById(orderRequest.ClientId);
            orderRequest.Client = client;
            var validReasonRequestIds = new List<int> { (int)ReasonRequestConst.Retiro, (int)ReasonRequestConst.CambioDeEquipo, (int)ReasonRequestConst.ServicioTecnico };

            if (validReasonRequestIds.Contains(request.ReasonRequestId) && reasonRequestId != (int)ReasonRequestConst.Reubicacion)
            {
                await this._orderService.SaveOrderRequestAssetsAsync(orderRequest.OrderRequestId, request.AssetId!.Value, request.CreatedBy, orderRequest);
            }

            await this._orderService.SaveOrderRequestStatusHistoryAsync(orderRequest.OrderRequestId, orderRequest.OrderStatusId, request.CreatedBy);

            if (request.Documents != null && request.Documents.Any())
            {
                await this._orderService.SaveOrderRequestDocumentsAsync(orderRequest.OrderRequestId, request.Documents, request.CreatedBy, request.UpdatedBy);
            }

            // Generar mensaje de notificación
            var notificationMessage = await this._notificationNewService.GenerateNotificationMessage(orderRequest);

            // Notificar al supervisor
            if (request.SupervisorId.HasValue)
            {
                await this._notificationNewService.NotifySupervisorAsync(request.SupervisorId.Value, notificationMessage, cancellationToken);
            }

            // Notificar al trade
            await this._notificationNewService.NotifyTradeAsync(orderRequest, notificationMessage, cancellationToken);

            return orderRequest;
        }
    }
}