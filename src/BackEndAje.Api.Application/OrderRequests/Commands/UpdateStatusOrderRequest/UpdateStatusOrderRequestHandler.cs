using BackEndAje.Api.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest
{
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateStatusOrderRequestHandler : IRequestHandler<UpdateStatusOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public UpdateStatusOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IClientAssetRepository clientAssetRepository, IHubContext<NotificationHub> hubContext)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._clientAssetRepository = clientAssetRepository;
            this._hubContext = hubContext;
        }

        public async Task<Unit> Handle(UpdateStatusOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = await this._orderRequestRepository.GetOrderRequestById(request.OrderRequestId);

            if (request.OrderStatusId == (int)OrderStatusConst.Aprobado && !orderRequest.OrderRequestDocuments.Any())
            {
                throw new InvalidOperationException("No se puede aprobar la solicitud sin documentos adjuntos.");
            }

            foreach (var orderRequestAsset in orderRequest.OrderRequestAssets)
            {
                var clientAssetDto = await this._clientAssetRepository
                    .GetClientAssetPendingApprovalByClientIdAndAssetIdAsync(orderRequest.ClientId, orderRequestAsset.AssetId);

                if (request.OrderStatusId == (int)OrderStatusConst.Rechazado && clientAssetDto == null)
                {
                    continue;
                }

                if (!IsValidOrderStatus(request.OrderStatusId))
                {
                    continue;
                }

                var clientAsset = this.CreateClientAssetFromDto(clientAssetDto, request);

                clientAsset.IsActive = request.OrderStatusId == (int)OrderStatusConst.Atendido;
                clientAsset.Notes = this.GetStatusNotes(request.OrderStatusId);

                await this._clientAssetRepository.UpdateClientAssetsAsync(clientAsset);
            }

            await this.UpdateOrderRequestStatus(request);

            if (orderRequest.Supervisor == null) return Unit.Value;
            var notificationMessage = $"El estado de la orden del cliente: {orderRequest.ClientCode} ha sido actualizado de estado.";

            await this._hubContext.Clients.User(orderRequest.Supervisor.UserId.ToString())
                .SendAsync("ReceiveMessage", "Sistema", notificationMessage, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        private ClientAssets CreateClientAssetFromDto(ClientAssets clientAssetDto, UpdateStatusOrderRequestCommand request)
        {
            return new ClientAssets
            {
                ClientAssetId = clientAssetDto.ClientAssetId,
                CediId = clientAssetDto.CediId,
                InstallationDate = request.StatusDate,
                ClientId = clientAssetDto.ClientId,
                AssetId = clientAssetDto.AssetId,
                CodeAje = clientAssetDto.CodeAje,
                CreatedAt = clientAssetDto.CreatedAt,
                CreatedBy = clientAssetDto.CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = request.CreatedBy,
            };
        }

        private bool IsValidOrderStatus(int orderStatusId)
        {
            return orderStatusId == (int)OrderStatusConst.Atendido ||
                   orderStatusId == (int)OrderStatusConst.Rechazado ||
                   orderStatusId == (int)OrderStatusConst.FalsoFlete ||
                   orderStatusId == (int)OrderStatusConst.Anulado;
        }

        private string GetStatusNotes(int orderStatusId)
        {
            return orderStatusId switch
            {
                (int)OrderStatusConst.Atendido => "Activo Atendido",
                (int)OrderStatusConst.Rechazado => "Activo Rechazado",
                (int)OrderStatusConst.FalsoFlete => "Activo tiene Falso Flete",
                (int)OrderStatusConst.Anulado => "Activo está Anulado",
                _ => throw new ArgumentOutOfRangeException(nameof(orderStatusId), orderStatusId, null)
            };
        }

        private async Task UpdateOrderRequestStatus(UpdateStatusOrderRequestCommand request)
        {
            await this._orderRequestRepository.UpdateStatusOrderRequestAsync(request.OrderRequestId, request.OrderStatusId, request.CreatedBy);

            var orderRequestStatusHistory = new OrderRequestStatusHistory
            {
                OrderRequestId = request.OrderRequestId,
                OrderStatusId = request.OrderStatusId,
                ChangeReason = request.ChangeReason,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
            };

            await this._orderRequestRepository.AddOrderRequestStatusHistoryAsync(orderRequestStatusHistory);
        }
    }
}
